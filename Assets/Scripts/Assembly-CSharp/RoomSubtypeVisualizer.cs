using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RoomSubtypeVisualizer : MonoBehaviour
{

    public void Start()
    {
        if (toggleButton_of_Floor)
        {
            toggleButton_of_Floor.OnToggle += Toggled;
        }
        if (propertiesMenu)
        {
            propertiesMenu.subtypeChanged += SubtypeChanged;
        }
        if (mouseListener)
        {
            mouseListener.OnEntered += O_EN;
            mouseListener.OnExited += O_EX;
        }
        SubtypeChanged();
        Toggled();
    }

    public void OnClick()
    {
        if (FloorTextBox) { FloorTextBox.text = this.FloorType; FloorTextBox.Rebuild(CanvasUpdate.PostLayout); }
        if (DescriptionTextBox) { DescriptionTextBox.text = this.currentInfo; DescriptionTextBox.Rebuild(CanvasUpdate.PostLayout); }
    }
    public void O_EN()
    {
        if (FloorTextBox) { FloorTextBox.text = this.FloorType; }
        if (DescriptionTextBox) { DescriptionTextBox.text = this.currentInfo; }

    }
    public void O_EX()
    {
        if (FloorTextBox) { FloorTextBox.text = "[None.]"; }
        if (DescriptionTextBox) { DescriptionTextBox.text = "[None.]"; }
    }

    public void SubtypeChanged()
    {
        int val;
        bool success = int.TryParse(propertiesMenu.visualSubtypeField.text, out val);
        if (success)
        {
            var thing = subTypeInfo.Where(self => self.Value == val);
            if (thing.Count() > 0)
            {
                var cont = thing.First();
                currentInfo = cont.Text;
                if (cont.sprite) { spriteRenderer.sprite = cont.sprite; }
                if (mouseListener.Hovered) { O_EN(); }
            }
            else if (val == -1)
            {
                currentInfo = "When at -1, will generate as a random VALID visual subtype.";
                spriteRenderer.sprite = IDKSprite;
                if (mouseListener.Hovered) { O_EN(); }
            }
            else if (val >= subTypeInfo.Count() | -1 > val)
            {
                currentInfo = N_AString;
                spriteRenderer.sprite = ErrorSprite;
                if (mouseListener.Hovered) { O_EN(); }
            }
        }
        Toggled();
    }

    public void Update()
    {
        bool b = false;
        foreach (ChamberButton button in propertiesMenu.chamberButtons.Values)
        {
            if (button.Toggled == true)
            {
                b = true;
            }
        }
        if (b == true) { hhh = true; }else { hhh = false; }
        Toggled();
    }
    private bool hhh;
    public void Toggled() 
    {

        if (spriteRenderer)
        {
            spriteRenderer.color = toggleButton_of_Floor.Toggled | hhh == false ? Color.white : new Color(0.1f, 0.1f, 0.1f, 1);
        }
    }




    public Text FloorTextBox;
    public Text DescriptionTextBox;


    public string currentInfo;

    public SpriteRenderer spriteRenderer;

    public string FloorType = "[Keep]";

    [SerializeField]
    public List<VisualContainer> subTypeInfo = new List<VisualContainer>() { };


    public RoomPropertiesMenu propertiesMenu;

    public ToggleButton toggleButton_of_Floor;

    public Sprite NotApplicableSprite;
    public Sprite ErrorSprite;
    public Sprite IDKSprite;

    public MouseListener mouseListener;

    public string N_AString = "This floor has no valid visual subtype for this value. The Appropriate floor WILL NOT generate if you use this subtype.";

    [Serializable]
    public class VisualContainer
    {
        public int Value = 0;
        public string Text = "PlaceHolder";
        public Sprite sprite;
    }
}
