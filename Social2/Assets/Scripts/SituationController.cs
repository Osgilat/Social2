using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SituationController : MonoBehaviour {

    public Situation currentSituation;
    public Situation previousSituation;


    public float timerToShowStateFor = 3.0f;
    public float initialTimer;

    private void Start()
    {
        initialTimer = timerToShowStateFor;
    }

    public void OnGUI()
    {
        int index = (int)currentSituation;
        GUI.Label(new Rect(10, 10, 100, 30), index.ToString());
        
    }


 

    private void Update()
    {
        if(currentSituation != previousSituation)
        {
            timerToShowStateFor -= Time.deltaTime;
        }
       
        if(timerToShowStateFor <= 0)
        {
            previousSituation = currentSituation;

            timerToShowStateFor = initialTimer;

            ResetStateIndex();
        }
    }

    public void ResetStateIndex()
    {
        currentSituation = Situation.DefaultState;
    }

    public enum Situation
    {
        DefaultState,

        //Сокровище достигнуто. + imp +
        AchievedTrueTreasure,

        //Рыцарь нашел и взял зелье. + imp +
        PotionPickup,

        // Рыцарь нашел и взял меч. + imp +
        SwordPickup,

        //  Рыцарь открыл ларец, но он оказался пустым. + imp +
        AchievedFakeTreasure,

        //  Рыцарь вошел в зеркало. + imp +
        TriggeredMirror,

        //  Полная победа с картинкой. + imp +
        WinnedRound,

        // Начало игры / раунда. + imp +
        RoundBegin,

        //  Раненый рыцарь выпил зелье. + imp + 
        DrinkedPotion,

        // Рыцарь получает ранение в бою. + imp +
        HurtByEnemy,

        //Безоружный рыцарь понял что скелет был призраком. + imp +
        GhostDissolvedWhenUnarmed,        

        //  Вооруженный рыцарь обманулся: скелет был призраком. + imp +
        GhostDissolvedWhenArmed,

        //  Рыцарь победил скелета в поединке. + imp +
        KilledEnemy,

        //  Рыцарь атакует скелета. + imp +
        AttackingEnemy,

        // Безоружный рыцарь увидел перед собой скелет. + imp +
        UnarmedSeeSkeleton,

        // Вооружённый рыцарь увидел перед собой скелет. + imp +
        ArmedSeeSkeleton,

        //  Рыцарь увидел сокровище.  + imp +
        SeeTreasure,

        //  Рыцаря убили + imp 
        KnightKilled
    }


    


}
