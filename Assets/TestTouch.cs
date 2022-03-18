using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTouch : MonoBehaviour
{
    bool isHorizontal = false;
    int chosenVector = 0;
    [SerializeField]
    bool swipeCoroutineIsRuning = false;
    InputController inputManager;
    Camera cameraMain;
    Vector2 startPosition;
    float startTime;
    private Coroutine Swipe;
    void Awake(){
        inputManager = InputController.Instance;
        cameraMain = Camera.main;
    }

    void OnEnable(){
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }
    void OnDisable(){
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }
    private void SwipeStart(Vector2 position, float time){
        startPosition = position;
        startTime = time;
        if (!swipeCoroutineIsRuning){
            Debug.Log(Utils.RoundVector2(inputManager.PrimaryPosition()));
            swipeCoroutineIsRuning = true;
            Swipe = StartCoroutine("SwipeDirectionCoroutine");
        }
    }

    private void SwipeEnd(Vector2 position, float time){
        Debug.Log("Swipe End");
        StopCoroutine("SwipeCoroutine");
        swipeCoroutineIsRuning = false;
    }
    private IEnumerator SwipeDirectionCoroutine(){
        while(true){
            if (System.Math.Abs(startPosition.x - inputManager.PrimaryPosition().x) > 0.5f || System.Math.Abs(startPosition.y - inputManager.PrimaryPosition().y) > 0.5f){
                if (System.Math.Abs(startPosition.x - inputManager.PrimaryPosition().x) > System.Math.Abs(startPosition.y - inputManager.PrimaryPosition().y)){
                    isHorizontal = true;
                    chosenVector = (int) System.Math.Round(inputManager.PrimaryPosition().y);
                }
                else {
                    isHorizontal = false;
                    chosenVector = (int) System.Math.Round(inputManager.PrimaryPosition().x);
                }
                StartCoroutine("SwipeCoroutine");
            }
            yield return null;    
        }
    }
    private IEnumerator SwipeCoroutine(){
        while(true){
            int dif = isHorizontal? (int) (Utils.RoundVector2(startPosition).x - Utils.RoundVector2(inputManager.PrimaryPosition()).x) :
                                    (int) (Utils.RoundVector2(startPosition).y - Utils.RoundVector2(inputManager.PrimaryPosition()).y);
            for (int i = 0; i < System.Math.Abs(dif); i++){
                BoardManager.MoveObjects(isHorizontal, chosenVector, System.Math.Sign(dif));
            }
            startPosition = inputManager.PrimaryPosition();
            yield return null;
        }
    }
}
