using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    enum Wait { Move, Planted, Watered, Harvested, PickedUp, Delivered, Enter }

    struct Step
    {
        public string text;
        public Wait wait;
        public Step(string t, Wait w) { text = t; wait = w; }
    }

    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text label;

    Step[] steps;
    int index;
    bool done;

    void Awake()
    {
        steps = new Step[]
        {
            new Step("Welcome to Loop Valley!\nWalk with W A S D and look around with the mouse.", Wait.Move),
            new Step("Press 1 to pick the seeds, stand next to a plot, and left-click to plant a crop.", Wait.Planted),
            new Step("Press 2 for the watering can and left-click the plot. Watering uses water from your tank (top left).", Wait.Watered),
            new Step("Solar panels make energy and the pump turns that energy into water. Keep an eye on the numbers at the top left.", Wait.Enter),
            new Step("When the crop turns yellow it is ripe. Press 3 for your hands and left-click to harvest it.", Wait.Harvested),
            new Step("Harvesting leaves crop residue. Walk up to the green cube and press E to pick it up.", Wait.PickedUp),
            new Step("Carry it to the compost bin and press E. Compost makes the soil richer, so crops grow faster.", Wait.Delivered),
            new Step("Animal manure is different: it goes to the orange biogas digester, where it makes energy. Plastic belongs in neither bin.", Wait.Enter),
            new Step("The helper robot collects waste for you and recharges when its battery is low. You can still beat it to the waste!", Wait.Enter),
            new Step("Your goal is shown at the top right. Harvest crops and divert waste before time runs out, and do not let waste pile up. Good luck!", Wait.Enter),
        };
    }

    void OnEnable()
    {
        GameEvents.CropPlanted += OnPlanted;
        GameEvents.CropWatered += OnWatered;
        GameEvents.CropHarvested += OnHarvested;
        GameEvents.ItemPickedUp += OnPickedUp;
        GameEvents.WasteDelivered += OnDelivered;
    }

    void OnDisable()
    {
        GameEvents.CropPlanted -= OnPlanted;
        GameEvents.CropWatered -= OnWatered;
        GameEvents.CropHarvested -= OnHarvested;
        GameEvents.ItemPickedUp -= OnPickedUp;
        GameEvents.WasteDelivered -= OnDelivered;
    }

    void Start()
    {
        index = 0;
        done = false;
        panel.SetActive(true);
        ShowStep();
    }

    void Update()
    {
        if (done) return;
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.kKey.wasPressedThisFrame)
        {
            Finish();
            return;
        }

        Wait w = steps[index].wait;
        if (w == Wait.Move)
        {
            if (kb.wKey.wasPressedThisFrame || kb.aKey.wasPressedThisFrame ||
                kb.sKey.wasPressedThisFrame || kb.dKey.wasPressedThisFrame) Next();
        }
        else if (w == Wait.Enter)
        {
            if (kb.enterKey.wasPressedThisFrame || kb.numpadEnterKey.wasPressedThisFrame) Next();
        }
    }

    void OnPlanted() { Signal(Wait.Planted); }
    void OnWatered() { Signal(Wait.Watered); }
    void OnHarvested() { Signal(Wait.Harvested); }
    void OnPickedUp(WasteItem item) { Signal(Wait.PickedUp); }
    void OnDelivered(WasteType type) { Signal(Wait.Delivered); }

    void Signal(Wait w)
    {
        if (!done && steps[index].wait == w) Next();
    }

    void Next()
    {
        index++;
        if (index >= steps.Length) Finish();
        else ShowStep();
    }

    void ShowStep()
    {
        string header = "<size=75%><color=#9FE3A0><b>TUTORIAL  " + (index + 1) + " / " + steps.Length + "</b></color></size>";
        string hint = steps[index].wait == Wait.Enter
            ? "\n<color=#FFD966>Press Enter to continue</color>"
            : "";
        label.text = header + "\n" + steps[index].text + hint
                   + "\n<size=70%><color=#B0B0B0>Press K to skip</color></size>";
    }

    void Finish()
    {
        done = true;
        panel.SetActive(false);
    }
}