using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameObject[] before = new GameObject[2];
    public static GameObject[] after = new GameObject[2];
    public static int[] last = new int[2];
    public static int[] first = new int[2];
    void Awake(){
    }

    public static void MoveObjects(int before, bool isHorizontal, int index, float sign){
        Board.Tile[] aux = CreateAux(isHorizontal, index);    
        int lenght = aux.Length;
        MoveLine(before, sign, lenght, isHorizontal, index, aux);
    }
    
    public static Board.Tile[] CreateAux(bool isHorizontal, int index){
        Board.Tile[] aux;
        int lenght;
        if (!isHorizontal){
            lenght = BoardManager2.board.tile[index].Length;
            aux = new Board.Tile[lenght];
        }
        else {
            lenght = BoardManager2.board.tile.Length;
            aux = new Board.Tile[lenght];
        }
        for (int k = 0; k < lenght; k++){
            aux[k] = BoardManager2.board.tile[isHorizontal?k: index][isHorizontal? index: k];  
        }  
        return aux;
    }

    private static void MoveLine(int bfInt, float sign, int lenght, bool isHorizontal, int index, Board.Tile[] auxVector){   
    //Representa um movimento:
        SetBefore(bfInt, lenght, auxVector[last[bfInt]].tileGameObject, isHorizontal);
        SetAfter(bfInt, lenght, auxVector[first[bfInt]].tileGameObject, isHorizontal);
        Vector3 bfPos = before[bfInt].transform.position;
        before[bfInt].transform.position = isHorizontal? new Vector3 (bfPos.x + sign, bfPos.y, bfPos.z): new Vector3 (bfPos.x, bfPos.y + sign, bfPos.z);
        Vector3 afPos = after[bfInt].transform.position;
        after[bfInt].transform.position = isHorizontal? new Vector3 (afPos.x + sign, afPos.y, afPos.z): new Vector3 (afPos.x, afPos.y + sign, afPos.z);
        for (int i = 0; i < lenght; i++){
            GameObject aux = auxVector[i].tileGameObject;
            Board.Tile boardTilePosition = BoardManager2.board.tile[isHorizontal?i: index][isHorizontal? index: i];
            Vector3 auxPos = aux.transform.position;
            float axisPosition = isHorizontal? auxPos.x : auxPos.y;
            if (axisPosition + sign >= -1 && axisPosition + sign < lenght){
                aux.transform.position = isHorizontal? new Vector3(auxPos.x + (sign), auxPos.y, auxPos.z) : new Vector3(auxPos.x, auxPos.y + (sign), auxPos.z);
            } else {
                if (sign < 0){
                    int j = i == 0? lenght - 1 : i - 1; //proximo a esquerda.
                    aux.transform.position = isHorizontal? new Vector3(auxVector[j].tileGameObject.transform.position.x + 1, auxPos.y, auxPos.z) : new Vector3(auxPos.x, auxVector[j].tileGameObject.transform.position.y + 1 + sign, auxPos.z);
                    last[bfInt] = i;
                    j = i == lenght - 1? 0 : i + 1;
                    first[bfInt] = j;
                    before[bfInt].transform.position = isHorizontal? new Vector3(auxVector[j].tileGameObject.transform.position.x - 1, auxPos.y, auxPos.z) : new Vector3(auxPos.x, auxVector[j].tileGameObject.transform.position.y -1 + sign, auxPos.z);
                    after[bfInt].transform.position = aux.transform.position + (isHorizontal? Vector3.right: Vector3.up);
                }
                else {
                    int j = i == lenght - 1? 0 : i + 1; //proximo a direita
                    aux.transform.position = isHorizontal? new Vector3(auxVector[j].tileGameObject.transform.position.x - 1 + sign, auxPos.y, auxPos.z) : new Vector3(auxPos.x, auxVector[j].tileGameObject.transform.position.y - 1 + sign, auxPos.z);
                    first[bfInt] = i; 
                    j = i == 0? lenght - 1 : i - 1;
                    last[bfInt] = j;
                    before[bfInt].transform.position = aux.transform.position + (isHorizontal? Vector3.left: Vector3.down);
                    after[bfInt].transform.position = isHorizontal? new Vector3(auxVector[j].tileGameObject.transform.position.x + 1 + sign, auxPos.y, auxPos.z) : new Vector3(auxPos.x, auxVector[j].tileGameObject.transform.position.y + 1 + sign, auxPos.z);                  
                }
            }
        }
    }

    public static void SetBefore(int bfInt, int lenght, GameObject lastGO, bool isHorizontal){
        if (before[bfInt] == null){
            before[bfInt] = GameObject.Instantiate(lastGO, 
                isHorizontal? new Vector3(lastGO.transform.position.x - lenght, lastGO.transform.position.y, lastGO.transform.position.z) : 
                            new Vector3(lastGO.transform.position.x, lastGO.transform.position.y - lenght, lastGO.transform.position.z), 
                            Quaternion.identity);
        } else {
            if (lastGO.tag != before[bfInt].tag) {
                Vector3 pos = before[bfInt].transform.position;
                GameObject.Destroy(before[bfInt]);
                before[bfInt] = GameObject.Instantiate(lastGO, pos, Quaternion.identity);
            }
            //before[bfInt].transform.position = isHorizontal? new Vector3(aux.transform.position.x - lenght, aux.transform.position.y, aux.transform.position.z) : 
            //                new Vector3(aux.transform.position.x, aux.transform.position.y - lenght, aux.transform.position.z);
        }
    }
    public static void SetAfter(int afInt, int lenght, GameObject firstGO, bool isHorizontal){
        if (after[afInt] == null){
            after[afInt] = GameObject.Instantiate(firstGO, 
                isHorizontal? new Vector3(firstGO.transform.position.x + lenght, firstGO.transform.position.y, firstGO.transform.position.z) : 
                            new Vector3(firstGO.transform.position.x, firstGO.transform.position.y + lenght, firstGO.transform.position.z), 
                            Quaternion.identity);
        } else {
            if (firstGO.tag != after[afInt].tag) {
                Vector3 pos = after[afInt].transform.position;
                GameObject.Destroy(after[afInt]);
                after[afInt] = GameObject.Instantiate(firstGO, pos, Quaternion.identity);
            }
        }
    }

    public static void Rearrange (int first, int index, Board.Tile[] aux, bool isHorizontal){
        int lenght = aux.Length;
        for (int i = 0; i < lenght; i++){
            int j = (i + first)%(lenght);
            //Debug.Log("j: " + j + " i "+ i + " first: " + first + " lenght: " + lenght);
            Transform auxTF = aux[j].tileGameObject.transform;
            aux[j].tileGameObject.transform.position = isHorizontal? new Vector3(i, auxTF.position.y, auxTF.position.z):
                                                      new Vector3(auxTF.position.x, i, auxTF.position.z);
            BoardManager2.board.tile[isHorizontal? i : index][isHorizontal? index: i] = aux[j];
        }
    }
    public static void DestroyBeforeAfter(){
        foreach (GameObject i in before){
            GameObject.Destroy(i);
        }
        foreach (GameObject i in after){
            GameObject.Destroy(i);
        }
    }
}
