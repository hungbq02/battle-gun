using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponReload : BaseMonoBehaviour
{
    [Header("Reload Settings")]
    public float reloadTime = 1f;
    public Image reloadCircle;
    public Text cooldownText;

    private float remainingTime;
    public bool IsReloading { get; private set; } = false;

    [SerializeField] private PlayerController playerController;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPlayerController();
        LoadReloadCircle();
        LoadCooldownText();

    }

    /// <summary>
    /// Reset state reload return to default.
    /// </summary>
    public void ResetReloadState()
    {
        IsReloading = false;
        if (reloadCircle) reloadCircle.gameObject.SetActive(false);
    }

    /// <summary>
    /// Tắt input reload của người chơi.
    /// </summary>
    public void DisableReloadInput()
    {
        playerController.input.reload = false;
    }

    /// <summary>
    /// Kiểm tra xem có thể reload không.
    /// Điều kiện: không đang reload và (đạn hết hiện tại & còn đạn trong magazine, hoặc nhấn nút reload, hoặc số đạn hiện tại không đủ cho 1 lần bắn).
    /// </summary>
    public bool CanReload(int currentAmmo, int magazineAmmo, int bulletPerTap)
    {
        return !IsReloading && ((currentAmmo == 0 && magazineAmmo > 0)
                || playerController.input.reload || currentAmmo < bulletPerTap);
    }

    /// <summary>
    /// Coroutine xử lý reload súng.
    /// </summary>
    public IEnumerator ReloadCoroutine(WeaponShooting weaponShooting)
    {
        IsReloading = true;
        AudioManager.Instance.PlaySFX("reload");

        playerController.SetAnimLayer("UpperBodyLayer", 1f);
        playerController.animator.SetBool("isReloading", true);

        // Hiển thị UI reload
        if (reloadCircle != null)
        {
            reloadCircle.gameObject.SetActive(true);
            remainingTime = reloadTime;
            reloadCircle.fillAmount = 1;
            cooldownText.text = reloadTime.ToString();
            reloadCircle.DOFillAmount(0, reloadTime);

            DOTween.To(() => remainingTime, x => remainingTime = x, 0f, reloadTime)
                .SetEase(Ease.Linear)
                .OnUpdate(() => cooldownText.text = remainingTime.ToString("F2"))
                .OnComplete(() => reloadCircle.gameObject.SetActive(false));
        }

        yield return new WaitForSeconds(reloadTime);

        CompleteReload(weaponShooting);
    }

    private void CompleteReload(WeaponShooting weaponShooting)
    {
        playerController.animator.SetBool("isReloading", false);

        // Tính số đạn cần reload
        int bulletsNeeded = weaponShooting.maxAmmo - weaponShooting.currentAmmo;
        // Update ammo
        // If there is enough bullets in magazine, full of current ammo
        if (weaponShooting.magazineAmmo >= bulletsNeeded)
        {
            weaponShooting.currentAmmo += bulletsNeeded;
            weaponShooting.magazineAmmo -= bulletsNeeded;
        }
        else
        {
            weaponShooting.currentAmmo += weaponShooting.magazineAmmo;
            weaponShooting.magazineAmmo = 0;
        }
        weaponShooting.UpdateAmmoUI();

        IsReloading = false;
        playerController.input.reload = false;
    }

    internal void SetReloadTime(float reloadTime)
    {
        this.reloadTime = reloadTime;
    }
    protected virtual void LoadPlayerController()
    {
        if (playerController != null) return;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    protected virtual void LoadReloadCircle()
    {
        if (reloadCircle != null) return;
        reloadCircle = GameObject.Find("ReloadCircle").GetComponent<Image>();
    }
    protected virtual void LoadCooldownText()
    {
        if (cooldownText != null) return;
        cooldownText = GameObject.Find("txtCountdown").GetComponent<Text>();
    }
}
