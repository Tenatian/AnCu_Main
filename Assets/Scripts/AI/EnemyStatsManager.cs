using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace SG
{
    public class EnemyStatsManager : CharacterStatsManager
    {
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyBossManager enemyBossManager;
        public UIEnemyHealthBar enemyHealthBar;
        EnemyManager enemyManager;

        PlayerManager playerManager;

        public DeadState deadState;

        public bool isBoss;

        private void Awake()
        {
            playerManager = FindObjectOfType<PlayerManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            enemyManager = GetComponent<EnemyManager>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        void Start()
        {
            if (!isBoss)
            {
                enemyHealthBar.SetMaxHealth(maxHealth);
            }
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public override void TakeDamageNoAnimation(int damage)
        {
            base.TakeDamageNoAnimation(damage);

            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }
        }

        public void BreakGuard()
        {
            enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
        }

        public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {

            base.TakeDamage(damage, damageAnimation = "Damage_01");

            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }

            enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            currentHealth = 0;
            enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
            isDead = true;
            enemyManager.currentState = deadState;

            if (isBoss && enemyBossManager != null)
            {
                Debug.Log("You defeated a boss!");
                //playerManager.HandlePlayerInCutscene();
                //transition to that boss's death scene
                //SceneManager.LoadScene(enemyBossManager.bossDeathScene);
            }
        }
    }
}