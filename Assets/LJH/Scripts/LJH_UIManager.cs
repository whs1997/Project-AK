using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LJH_UIManager : MonoBehaviour
{
    [Header("스크립트")]
    [Header("데미지 매니저 스크립트")]
    [SerializeField] LJH_DamageManager damageManager;

    [Header("쉴드 내구도 UI")]
    [SerializeField] GameObject[] ljh_shieldImages;     // 내구도 UI용

    [Header("최대 체력")]
    private float ljh_MaxHP = 10000;
    
    [Header("체력바 색")]
    private Color ljh_curColor;
    private readonly Color ljh_initColor = Color.green;
    
    [Header("현재 체력")]
    [Range (0,10000)]
    [SerializeField] public float ljh_curHp;

    [Header("체력 비율")]
    [SerializeField] public float hpPercentage;

    [Header("체력바 이미지")]
    [SerializeField] public Image ljh_hpBar;

    [Header("체력 퍼센트 텍스트")]
    [SerializeField] public TextMeshProUGUI hpText;

    [Header("플레이어 사망 체크용")]
    [SerializeField] bool isDie;

    private static LJH_UIManager instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static LJH_UIManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Start()
    {
        ljh_curColor = ljh_initColor;
        ljh_hpBar.color = ljh_initColor;

        isDie = false;
    }


    private void Update()
    {
        ljh_curHp = damageManager.GetComponent<LJH_DamageManager>().ljh_curHp;
        DisplayHpBar();
        
        if (!isDie)
        {
            if (ljh_curHp <= 0)
            {
                PlayerDied();
            }
        }
    }

    private void PlayerDied()
    {
        LSY_SceneManager.Instance.PlayerDied();
        isDie = true;
    }

    public void UpdateShieldUI(float durability)
    {
        durability = Mathf.Clamp(durability, 0, ljh_shieldImages.Length);
        for (int i = 0; i < ljh_shieldImages.Length; i++)
        {
            ljh_shieldImages[i].gameObject.SetActive(i < durability);
        }
    }

    public void DisplayHpBar()
    {
        hpPercentage = ljh_curHp / ljh_MaxHP;
        if (hpPercentage > 0.5f)
        {
            ljh_curColor = Color.green;
        }
        else if (hpPercentage > 0.3f)
        {
            ljh_curColor = Color.yellow;
        }
        else
        {
            ljh_curColor = Color.red;
        }
        ljh_hpBar.color = ljh_curColor;
        ljh_hpBar.fillAmount = hpPercentage;

        if (hpPercentage < 0)
        {
            hpText.text = "0%";
        }
        else if(hpPercentage >= 1)
        {
            hpText.text = "100%";
        }
        else
        {
            hpText.text = (hpPercentage * 100).ToString("F0") + "%";
        }
    }
}