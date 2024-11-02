using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class HYJ_Boss_Stage1 : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] GameObject player;
    [Header("보스 설정")]
    [SerializeField] Animator animator;
    [SerializeField] GameObject monster;
    [SerializeField] float nowHp;
    [SerializeField] float SetHp;
    //[SerializeField] float 
    [SerializeField] public float monsterShieldAtkPower;
    [SerializeField] public float monsterHpAtkPower;
    [SerializeField] public float monsterMoveSpeed;

    [Header("임의 변수")]
    [SerializeField] public bool hitFlag;
    public bool HitFlag { get { return hitFlag; } set { hitFlag = value; } }
    public bool isAttack;
    public bool nowAttack;
    public bool isDie;
    Coroutine hitFlagCoroutine;
    WaitForSeconds hitFlagWaitForSeconds = new WaitForSeconds(0.1f);
    private bool firstBattleEnd=false;
    private bool pFirst = false;
    private bool pSecond = false;
    private bool p10 = false;
    private bool p40 = false;
    private bool p70 = false;
    [SerializeField] float xNow = 0;
    [SerializeField] float xMoveDirection = 0.1f;
    private bool isSiuu = false;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.tag = "Boss";
        SetHp = 3500f;
        monsterMoveSpeed = 1.5f;
        nowHp = SetHp;
    }

    private void Update()
    {
        MonsterDie();
        if (!isSiuu)
        {
            BossMove();
        }
        if (!firstBattleEnd && !pFirst && !pSecond)
        {
            StartCoroutine(BossBattleStart());
        }
        else if (firstBattleEnd && pFirst && pSecond)
        {
            StartCoroutine(BossAI());
        }
    }

    // Comment : 헤드스핀 패턴
    private void PatternHeadSpin()
    {
        Debug.Log("헤드스핀");
        monsterShieldAtkPower = 4000f;
        monsterHpAtkPower = 5f;
        animator.SetTrigger("HeadSpin");
        nowAttack = true;

    }

    // Comment : 브레이크댄스 패턴
    private void PatternBreakDance()
    {
        Debug.Log("브레이크댄스");
        monsterShieldAtkPower = 1000;
        monsterHpAtkPower = 3;
        animator.SetTrigger("BreakDance");
        nowAttack = true;

    }

    // Comment : 세레모니 패턴
    private void PatternSiiuuuu()
    {
        isSiuu = true;
        Debug.Log("세레머니");
        monsterShieldAtkPower = 3000f;
        monsterHpAtkPower = 1f;
        animator.SetTrigger("Siuu");
        nowAttack = true;
        Vector3 bossPos = monster.transform.position;
        //monster.transform.
    }

    // Comment : 보스 죽음 패턴
    private void MonsterDie()
    {
        if (nowHp <= 0)
        {
            //사망 애니메이션
            //monsterAnimator.SetTrigger("Die");
            Debug.Log("사망");
            Destroy(gameObject, 2f);
        }
    }

    // Comment : 보스 조우 패턴 
    IEnumerator BossBattleStart()
    {
        Debug.Log("연습");
        if (!pFirst)
        {
            // Comment : 
            Debug.Log("보스 학습용 첫 패턴으로 브레이크 댄스를 한다.");
            PatternBreakDance();
            pFirst = true;
            yield return new WaitForSeconds(4f);
        }
        if (pFirst && !pSecond)
        {
            // Comment : 
            Debug.Log("보스 학습용 두번째 패턴으로 세레모니 패턴을 한다.");
            PatternSiiuuuu();
            pSecond = true;
            yield return new WaitForSeconds(4f);
            firstBattleEnd = true;
        }
    }


    // Comment : 
    IEnumerator BossAI()
    {
        if (nowHp < 2450f && !p70)
        {
            // Comment : 보스 HP가 처음으로 70퍼 아래가 되어 헤드스핀을 사용한다.
            p70 = true;
            PatternHeadSpin();
            Debug.Log("보스 HP가 처음으로 70퍼 아래가 되어 헤드스핀을 사용한다.");
            yield return new WaitForSeconds(4f);
        }
        else if (nowHp < 1400f && !p40)
        {
            // Comment : 보스 HP가 처음으로 40퍼 아래가 되어 헤드스핀을 사용한다."
            p40 = true;
            PatternHeadSpin();
            Debug.Log("보스 HP가 처음으로 40퍼 아래가 되어 헤드스핀을 사용한다.");
            yield return new WaitForSeconds(4f);
        }
        else if (0<nowHp&&nowHp < 350f && !p10)
        {
            // Comment : 보스 HP가 처음으로 10퍼 아래가 되어 헤드스핀을 사용한다.
            p10 = true;
            PatternHeadSpin();
            Debug.Log("보스 HP가 처음으로 10퍼 아래가 되어 헤드스핀을 사용한다.");
            yield return new WaitForSeconds(4f);
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

    void BossMove()
    {
        
        float xMax = 8f;
        float xMin = -8f;
        

        xNow += xMoveDirection;
        monster.transform.position = new Vector3(xNow, monster.transform.position.y, monster.transform.position.z);

        if (xNow >= xMax)
        {
            Debug.Log("방향 전환");
            Debug.Log(xMoveDirection);
            xMoveDirection = -Time.deltaTime * 3f;
        }
        else if(xNow <= xMin)
        {
            xMoveDirection = Time.deltaTime * 3f;
        }
    }
}
