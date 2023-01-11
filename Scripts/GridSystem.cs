using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSystem : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject referenceTile;
    public List<Tile> allTiles;
    public GridLayoutGroup gridLayout;
    public int tileTotal;  
    public int totalClicked;
    public int totalBombCount;

    public List<Sprite> allSprites;
    //0 flag, 1 bomb
    public List<Color> allColors;
    // 0 normal, 1 clicked, 2 highlight, 3-> pretty

    public void GenerateNewGrid(Vector2 newGridSize, int bombCount){
        DestroyOldGrid();
        var gridX = (int)(newGridSize.x);
        gridLayout.constraintCount = gridX;
        tileTotal = (int)(newGridSize.x) * (int)(newGridSize.y);
        totalBombCount = bombCount;
        totalClicked = tileTotal - totalBombCount;
        var currentX = 0;
        var currentY = 0;
        for(int x = 0; x < tileTotal; x++){
            var newObj = Instantiate(referenceTile, gridLayout.gameObject.transform);
            var newTile = newObj.GetComponent<Tile>();
            newTile.myPosition = new Vector2(currentX, currentY);
            currentY += 1;
            if(currentY >= gridX){currentX += 1;currentY = 0;}
            newObj.SetActive(true);
            allTiles.Add(newTile);
        }
        AssignBombs();
    }

    public void DestroyOldGrid(){
        if(allTiles.Count > 0){
            foreach(Tile newTile in allTiles){
                GameObject.Destroy(newTile.gameObject);
            }
        }
        allTiles.Clear();
    }

    public void AssignBombs(){
        var tempBombCount = 0;
        while(tempBombCount < totalBombCount){
            var getRandom = UnityEngine.Random.Range(0, tileTotal);
            if(!allTiles[getRandom].hasBomb){
                tempBombCount += 1;
                allTiles[getRandom].AssignBomb();
            }
        }
    }

    public void ShowAllBombs(){
        foreach(Tile newTile in allTiles){
            if(newTile.hasBomb && !newTile.hasFlag){
                newTile.ShowBomb();
            }
        }
    }

    public void GetNeighbors(Tile pickedTile){
        if(!pickedTile.isCounted){
            totalClicked -= 1;
            pickedTile.isCounted = true;
        }
        var surroundingTiles = new List<Tile>();
        foreach(Tile newTile in allTiles){
            //lol sorry, it works though!
            if((newTile.myPosition.x == pickedTile.myPosition.x - 1 && newTile.myPosition.y == pickedTile.myPosition.y - 1) ||
            (newTile.myPosition.x == pickedTile.myPosition.x - 1 && newTile.myPosition.y == pickedTile.myPosition.y) ||
            (newTile.myPosition.x == pickedTile.myPosition.x - 1 && newTile.myPosition.y == pickedTile.myPosition.y + 1) ||
            (newTile.myPosition.x == pickedTile.myPosition.x && newTile.myPosition.y == pickedTile.myPosition.y - 1) ||
            (newTile.myPosition.x == pickedTile.myPosition.x && newTile.myPosition.y == pickedTile.myPosition.y + 1) ||
            (newTile.myPosition.x == pickedTile.myPosition.x + 1 && newTile.myPosition.y == pickedTile.myPosition.y - 1) ||
            (newTile.myPosition.x == pickedTile.myPosition.x + 1 && newTile.myPosition.y == pickedTile.myPosition.y) ||
            (newTile.myPosition.x == pickedTile.myPosition.x + 1 && newTile.myPosition.y == pickedTile.myPosition.y + 1)){
                surroundingTiles.Add(newTile);
            } 
        }
        var bombCount = 0;
        foreach(Tile newerTile in surroundingTiles){
            if(newerTile.hasBomb){bombCount += 1;}
        }
        if(bombCount > 0){
            pickedTile.NeighborHasBomb(bombCount);  
            if(totalClicked <= 0){gameManager.GameOver(0);}
            return;}
        if(totalClicked <= 0){gameManager.GameOver(0);}
        pickedTile.oldColor = allColors[1];
        foreach(Tile newestTile in surroundingTiles){
            newestTile.ClickEffect();
        }

    }
}