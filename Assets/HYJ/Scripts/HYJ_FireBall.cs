using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYJ_FireBall : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] public float nowHp;
    [SerializeField] public float setHp;
    [SerializeField] public bool hitFlag;
    public bool HitFlag { get { return hitFlag; } set { hitFlag = value; } }
    Coroutine hitFlagCoroutine;
    WaitForSeconds hitFlagWaitForSeconds = new WaitForSeconds(0.1f);

    [SerializeField] GameObject damageManager;

    void Start()
    {
        setHp = 200;
        nowHp = setHp;
        Destroy(gameObject, 3f);
        player = GameObject.FindGameObjectWithTag("Player");
        damageManager = GameObject.FindGameObjectWithTag("DamagerManager");
    }

    void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(player.transform.position.x, player.transform.position.y - 0.3f, player.transform.position.z), 0.1f);
        if (nowHp <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void MonsterTakeDamageCalculation(float damage)
    {
        nowHp -= damage;
    }
    public void StartHitFlagCoroutine()
    {
        if (hitFlagCoroutine != null)
        {
            StopCoroutine(hitFlagCoroutine);
        }
        hitFlagCoroutine = StartCoroutine(HitFlagCoroutine());
    }

    IEnumerator HitFlagCoroutine()
    {
        yield return hitFlagWaitForSeconds;
        hitFlag = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 만약 안되면 위에 Damage매니저로 변경
            damageManager.GetComponent<LJH_DamageManager>().BossTakeBallDamage(3000, 1);
        }
    }
}
