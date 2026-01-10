using UnityEngine;

public class CollisionPlayerToEnemy : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private LooseMenuUI looseMenuUI;
    [SerializeField] private WinMenuUI winMenuUI;
    #endregion
    #region Private Variables
    private int collisions = 0;
    #endregion
    #region Functions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!pauseManager.IsPaused && Time.timeScale == 0f) return;
            pauseManager.PauseWithoutUI();
            looseMenuUI.LooseMenuObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                collisions++;
                if (collisions > 1)
                {
                    collisions = 0;
                    if (!pauseManager.IsPaused && Time.timeScale == 0f) return;
                    pauseManager.PauseWithoutUI();
                    looseMenuUI.LooseMenuObject.SetActive(true);
                }
                break;
            case "Finish":
                if (!pauseManager.IsPaused && Time.timeScale == 0f) return;
                pauseManager.PauseWithoutUI();
                winMenuUI.WinMenuObject.SetActive(true);
                break;
        }
    }
    #endregion
}
