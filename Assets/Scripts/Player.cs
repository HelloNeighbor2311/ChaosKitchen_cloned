using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IKitChenObjectParent
{
    public static Player Instance { get; private set; }

    // Event để thông báo khi selectedCounter thay đổi
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    // Class để truyền dữ liệu khi sự kiện OnSelectedCounterChanged được gọi
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    public event EventHandler pickup;
    

    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    


    private BaseCounter selectedCounter;
    private float speed = 7f;
    private float rotationSpeed = 10f;
    private bool isWalking = false;
    float playerRadius = .7f;
    float playerHeight = 2f;
    private Vector3 lastInteractDir;
    private KitchenObject kitchenObject;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }
    private void Start()
    {
        //Gán sự kiện thao tác với các nút
        gameInput.onInteractAction += GameInput_OnInteractAction;
        gameInput.onInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void Update()
    {
        HandleMovement();
        HandleInteract();

    }

    //Xử lý sự kiện của OnInteractAlternateAction
    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.isGamePlaying())
        {
            return;
        }
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }

    }
    //Tương tự với OnInteractAction
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.isGamePlaying()){
            return;
        }
        
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }

    }

    private void HandleInteract()
    {
        var inputVector = gameInput.MovementNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y); 

        float interactDistance = 1.3f; //Limit khoảng cách tối đa có thể bắn raycast

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        //Bắn raycast
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit rayCastinfo, interactDistance, counterLayerMask))
        {
            //Tìm các object có script BaseCounter
            if (rayCastinfo.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
       
    }
    private void HandleMovement()
    {
        var inputVector = gameInput.MovementNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = speed * Time.deltaTime;
        
        //Capsule cast, tương tự raycast nhưng tính toán thêm cả kích thước của bản thân, ở đây, canMove sẽ tính toán liệu người chơi có thể di chuyển
        var canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);


        if (!canMove)
        {
            //không thể đi thẳng theo vị trí di chuyển

            //thử di chuyển theo mỗi trục x
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //Nếu có thể di chuyển theo trục x
                moveDir = moveDirX;
            }
            else
            {
                //Ngược lại

                //thử di chuyển theo mỗi trục z
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    //Nếu có thể di chuyển theo trục z
                    moveDir = moveDirZ;
                }
                else { }
            }
        }



        if (canMove)
        {
            transform.position += moveDir * moveDistance;

        }
        isWalking = moveDir != Vector3.zero;
        IsWalking();
        //Xoay nhân vật theo hướng của phím di chuyển
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
    }
    public bool IsWalking()
    {
        return isWalking;
    }

    // Phương thức để thiết lập selectedCounter và gọi sự kiện khi nó thay đổi
    private void SetSelectedCounter(BaseCounter clearCounter)
    {
        selectedCounter = clearCounter;
        // Gọi sự kiện OnSelectedCounterChanged và truyền selectedCounter mới
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
            // truyền selectedCounter được khai báo ban đầu vào field bên trong lớp  OnSelectedCounterChangedEventArgs
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null) { 
            pickup?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
