using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public GridSystem gridSystem;
    public TMP_Text timerText;
    public TMP_Text startButtonText;
    public List<TMP_InputField> sizeInputFields;
    public TMP_InputField bombInputField;
    public GameObject errorMessageObject;
    //
    public Vector2 gridSize;
    public int bombCount;
    public float currentTime = 0f;
    public Coroutine timerCoroutine;
    public bool doTime;
    public bool gameOver;


    public void DoStartButton(){
        StopTimer();
        var totalTiles = gridSize.x * gridSize.y;
        if(bombCount >= totalTiles){
            ErrorMessage("Too many bombs for this grid, " + bombCount + " bombs with " + totalTiles + " tiles.");
            foreach(TMP_InputField newIP in sizeInputFields){
                newIP.text = "" + 9;
            }
            gridSize = new Vector2(9, 9);
            bombCount = 9;
            bombInputField.text = "" + 9;
            return;
        }

        startButtonText.text = "Restart";
        timerText.text = "00:00";
        gridSystem.GenerateNewGrid(gridSize, bombCount);
        gameOver = false;
    }


    public void InputSize(int index){
        var newSize = int.Parse(sizeInputFields[index].text);
        if(newSize <= 0 || newSize > 15){
            ErrorMessage("Sizes must be greater than 0, less than 15");
            newSize = 9;
            sizeInputFields[index].text = "" + newSize;
            }
        if(index == 0){gridSize.x = newSize;}
        if(index == 1){gridSize.y = newSize;}
    }

    public void InputBomb(){
        var newBombs = int.Parse(bombInputField.text);
        if(newBombs <= 0 || newBombs > 99){
            ErrorMessage("Bombs must be greater than 0, less than 99");
            newBombs = 9;
            bombInputField.text = "" + newBombs; 
            }
        bombCount = newBombs;
    }

    public void GameOver(int winLose){
        // 0 win 1 lose
        gameOver = true;
        StopTimer();
        if(winLose == 1){
            ErrorMessage("YOU LOSE! TRY AGAIN!");
            gridSystem.ShowAllBombs();
        }
        if(winLose == 0){
            ErrorMessage("YOU WIN! PLAY AGAIN!");
        }
    }

    public void ErrorMessage(string newMessage){
        errorMessageObject.GetComponent<TMP_Text>().text = newMessage;
        errorMessageObject.SetActive(true);
        Invoke("FinishError", 3.0f);
    }

    public void FinishError(){
        errorMessageObject.SetActive(false);
    }

    public void StartTimer(){
        currentTime = 0f;
        doTime = true;
        timerCoroutine = StartCoroutine(TimerCo());
    }

    public void StopTimer(){
        doTime = false;
        if(timerCoroutine != null){
        StopCoroutine(timerCoroutine);
        }
    }

    public IEnumerator TimerCo(){
        while(doTime){
        yield return new WaitForSeconds(1f);
        currentTime += 1f;
        float minutes = Mathf.FloorToInt(currentTime / 60); 
        float seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

}
