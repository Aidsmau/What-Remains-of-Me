using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotOutlet : AControllable
{
    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("inkJSON")]
    [SerializeField] private TextAsset atlasLowEnemyLowVirusScript;
    [SerializeField] private TextAsset atlasLowEnemyMediumVirusScript;
    [SerializeField] private TextAsset atlasLowEnemyHighVirusScript;
    [SerializeField] private TextAsset atlasHighEnemyLowVirusScript;
    [SerializeField] private TextAsset atlasHighEnemyMediumVirusScript;
    [SerializeField] private TextAsset atlasHighEnemyHighVirusScript;

    [Header("Enemy Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite deadRobot;
    [SerializeField] private Sprite chargedRobot;
    [SerializeField] private Sprite virusRobot;

    [Header("Dialogue Trigger Configs")]
    [SerializeField] private float dialogueActivateDistance = 5f;
    [SerializeField] private bool stopMovement = true;
    [SerializeField] private bool autoTurnPage = false;
    [SerializeField] private float waitForPageTurn = 2f;
    [SerializeField] private float waitForInteractAvailable = 3f;
    [SerializeField] private int virusLevelUpdate;

    private TextAsset currentInkJSONScript;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // this if is an intial question to check energy state of the robot, and see if they are active. If you wanted more states 
        // of energy, bring the virus switch case into a helper function, and replace the if statement with a switch statement for energy.
        // You can bring the virus switch case into a hepler function, and call this from the energy switch case. The idea is you have
        // different states of energy, changing how the robot reacts. Then on top of the energy, there is a virus switch case checking how
        // behavior should change depending on virus

        // update sprite based on current virus/clean energy level
        if (cleanEnergy > 1 || virus > 1)
        {
            spriteRenderer.sprite = chargedRobot;
        }
        else
        {
            spriteRenderer.sprite = deadRobot;
        }

        // Update ink script for enemy to say
        if (playerInfo.virus <= 50) // atlas has low virus level
        {
            if (virus < 50) // enemy has low virus level
            {
                currentInkJSONScript = atlasLowEnemyLowVirusScript;
            }
            else if (virus >= 50 && virus < 80) // enemy has medium virus level
            {
                currentInkJSONScript = atlasLowEnemyMediumVirusScript;
            }
            else // enemy has high virus level
            {
                currentInkJSONScript = atlasLowEnemyHighVirusScript;
            }

        }
        else // atlas has high virus level
        {
            if (virus < 50)
            {
                currentInkJSONScript = atlasHighEnemyLowVirusScript;
            }
            else if (virus >= 50 && virus < 80) // enemy has medium virus level
            {
                currentInkJSONScript = atlasHighEnemyMediumVirusScript;
            }
            else // atlas has high virus level
            {
                currentInkJSONScript = atlasHighEnemyHighVirusScript;
            }
        }

        // play script if player is within activate distance
        if (PlayerInTriggerDistance() && !InkDialogueManager.GetInstance().dialogueIsPlaying)
        {
            var i = InkDialogueManager.GetInstance();
            i.EnterDialogueMode(currentInkJSONScript);
            i.stopMovement = this.stopMovement;
            i.autoTurnPage = this.autoTurnPage;
        }


    }

    private bool PlayerInTriggerDistance()
    {
        return Vector2.Distance(transform.position, player.transform.position) < dialogueActivateDistance;
    }
}

