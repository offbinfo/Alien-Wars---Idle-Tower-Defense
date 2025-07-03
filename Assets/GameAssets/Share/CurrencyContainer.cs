using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyContainer : Singleton<CurrencyContainer>
{
    public Transform _trans_gold => txt_gold.transform;
    public Transform _trans_gem => txt_gem.transform;
    public Transform _trans_badges => txt_Bades.transform;
    [SerializeField] TMP_Text txt_silver;
    [SerializeField] TMP_Text txt_gold;
    [SerializeField] TMP_Text txt_gem;
    [SerializeField] TMP_Text txt_Bades;
    public Transform trans_gold;

}
