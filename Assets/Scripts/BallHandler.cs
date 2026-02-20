using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class BallHandler : MonoBehaviour
{
    [SerializeField]GameObject BallPrefabs;
    [SerializeField] Rigidbody2D Pivot;
    [SerializeField]float RespawnDelay;
    [SerializeField]float DetachTime=0.5f;
    Rigidbody2D CurrentBallRigidbody;
    SpringJoint2D CurrentBallSpringJoint;
    Camera MainCamera;
    bool IsDragging;
    void Start()
    {
        MainCamera=Camera.main;
        SpawnNewBall();
    }

   
    void Update()
    {

       if(CurrentBallRigidbody==null){
return;
        }
        if(!Touchscreen.current.primaryTouch.press.isPressed){
            if(IsDragging){
            LaunchBall();
            }
            IsDragging=false;
           
            return;
        }
        IsDragging=true;
       CurrentBallRigidbody.isKinematic=true;
        Vector2 TouchPosition=Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 WorldPosition=MainCamera.ScreenToWorldPoint(TouchPosition);
        CurrentBallRigidbody.position=WorldPosition;
         
    }
    void LaunchBall(){
CurrentBallRigidbody.isKinematic=false;
CurrentBallRigidbody=null;
Invoke("Detach",DetachTime);
    }
   void Detach(){
CurrentBallSpringJoint.enabled=false;
CurrentBallSpringJoint=null;
Invoke("SpawnNewBall",RespawnDelay);
    }
    void SpawnNewBall(){
       GameObject BallInstance= Instantiate(BallPrefabs,Pivot.position,Quaternion.identity);
       CurrentBallRigidbody=BallInstance.GetComponent<Rigidbody2D>();
        CurrentBallSpringJoint=BallInstance.GetComponent<SpringJoint2D>();
        CurrentBallSpringJoint.connectedBody=Pivot;
    }
}
