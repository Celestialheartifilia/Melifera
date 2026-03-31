using UnityEngine;
using UnityEngine.SceneManagement;

public class Customers : MonoBehaviour
{
    public static Customers Instance;

    public GameObject customer1;
    public GameObject customer2;
    public GameObject customer3;

    //int currentCustomerIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnNextCustomer();
    }

    public void SpawnNextCustomer()
    {
        OrderTakingManager manager = OrderTakingManager.Instance;
        manager.currentCustomerIndex++;

        if (manager.currentCustomerIndex > manager.maxCustomers)
        {
            Debug.Log("All customers finished.");
            return;
        }

        int index = manager.currentCustomerIndex;

        // Hide all customers
        if (customer1) customer1.SetActive(false);
        if (customer2) customer2.SetActive(false);
        if (customer3) customer3.SetActive(false);

        if (index == 1)
        {
            customer1.SetActive(true);
            //manager.CreateNewOrder(OrderTakingManager.OrderType.Small);
            manager.pendingOrderType = OrderTakingManager.OrderType.Small;
        }
        else if (index == 2)
        {
            customer2.SetActive(true);
            //manager.CreateNewOrder(OrderTakingManager.OrderType.Medium);
            manager.pendingOrderType = OrderTakingManager.OrderType.Medium;

        }
        else if (index == 3)
        {
            customer3.SetActive(true);
            //manager.CreateNewOrder(OrderTakingManager.OrderType.Big);
            manager.pendingOrderType = OrderTakingManager.OrderType.Big;
        }

        OrderUIManager ui = FindObjectOfType<OrderUIManager>();
        if (ui) ui.RefreshUIForNewOrder();

    }
}
