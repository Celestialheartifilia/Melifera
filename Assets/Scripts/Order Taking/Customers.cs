using UnityEngine;

public class Customers : MonoBehaviour
{
    public static Customers Instance;

    [Header("Customer GameObjects")]
    public GameObject customer1;
    public GameObject customer2;
    public GameObject customer3;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        OrderTakingManager manager = OrderTakingManager.Instance;

        if (manager == null)
        {
            Debug.LogError("OrderTakingManager missing.");
            return;
        }

        // First time entering the scene
        if (manager.currentCustomerIndex == 0)
        {
            SpawnNextCustomer();
            return;
        }

        // If order still exists, restore same customer
        if (manager.currentOrder != null)
        {
            RestoreCurrentCustomer();
        }
        else
        {
            // Order finished earlier, spawn next one
            SpawnNextCustomer();
        }
    }

    public void SpawnNextCustomer()
    {
        OrderTakingManager manager = OrderTakingManager.Instance;

        if (manager == null)
            return;

        if (!manager.HasMoreCustomers())
        {
            Debug.Log("All customers finished.");
            return;
        }

        manager.currentCustomerIndex++;

        int index = manager.currentCustomerIndex;

        HideAllCustomers();

        if (index == 1)
        {
            customer1.SetActive(true);
            manager.CreateNewOrder(OrderTakingManager.OrderType.Small);
        }
        else if (index == 2)
        {
            customer2.SetActive(true);
            manager.CreateNewOrder(OrderTakingManager.OrderType.Medium);
        }
        else if (index == 3)
        {
            customer3.SetActive(true);
            manager.CreateNewOrder(OrderTakingManager.OrderType.Big);
        }

        RefreshUI();
    }

    void RestoreCurrentCustomer()
    {
        int index = OrderTakingManager.Instance.currentCustomerIndex;

        HideAllCustomers();

        if (index == 1 && customer1 != null)
            customer1.SetActive(true);

        else if (index == 2 && customer2 != null)
            customer2.SetActive(true);

        else if (index == 3 && customer3 != null)
            customer3.SetActive(true);

        RefreshUI();
    }

    void HideAllCustomers()
    {
        if (customer1) customer1.SetActive(false);
        if (customer2) customer2.SetActive(false);
        if (customer3) customer3.SetActive(false);
    }

    void RefreshUI()
    {
        OrderUIManager ui = FindObjectOfType<OrderUIManager>();

        if (ui != null)
            ui.RefreshUIForNewOrder();
    }
}