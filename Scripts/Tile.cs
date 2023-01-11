using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using TMPro;

public class Tile : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameManager gameManager;
    public GridSystem gridSystem;
    public Vector2 myPosition;
    public Image myImage;
    public TMP_Text tileText;
    public Image tileImage;

    public bool hasFlag;
    public bool hasBomb;
    public bool beenClicked;

    public Color oldColor;
    public bool isCounted;

    public void OnPointerClick (PointerEventData eventData) {
        
        if(gameManager.gameOver){return;}
        if(!gameManager.doTime){gameManager.StartTimer();}
         if (eventData.button == PointerEventData.InputButton.Right){
                if(tileText.isActiveAndEnabled == true){return;}
             if(hasFlag){
                tileImage.sprite = null;
                tileImage.gameObject.SetActive(false);
                hasFlag = false;
                return;
             }
             if(!hasFlag){
                tileImage.sprite = gridSystem.allSprites[0];
                tileImage.gameObject.SetActive(true);
                hasFlag = true;
                return;
             }
         }
         if (eventData.button == PointerEventData.InputButton.Left){
             if(beenClicked){beenClicked = false;}
             ClickEffect();

         }
     }

     public void OnPointerEnter (PointerEventData eventData) {
        oldColor = myImage.color;
        myImage.color = gridSystem.allColors[2];
     }

     public void OnPointerExit (PointerEventData eventData){
        myImage.color = oldColor;
     }

     public void ClickEffect(){
        if(beenClicked){return;}
        if(hasFlag){return;}
             if(hasBomb){
                gameManager.GameOver(1);
                ShowBomb();
                return;
                }
            beenClicked = true;

            myImage.color = gridSystem.allColors[1];
            gridSystem.GetNeighbors(this);
     }

     public void NeighborHasBomb(int bombCount){
        tileText.gameObject.SetActive(true);
        myImage.color = gridSystem.allColors[0];
        oldColor = gridSystem.allColors[0];
        tileText.text = "" + bombCount;
        tileText.color = gridSystem.allColors[bombCount + 2];
     }

     public void AssignBomb(){
        hasBomb = true;
     }

     public void ShowBomb(){
        tileImage.sprite = gridSystem.allSprites[1];
        tileImage.gameObject.SetActive(true);
     }

}
