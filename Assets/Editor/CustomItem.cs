using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Define;
public class CustomItem : EditorWindow
{
    public string[] spriePath;

    public VisualElement m_RightPane;
    public ItemType itemType;
    public string itemName;
    public string itemInfo;
    public Sprite sprite;

    HealType addType;
    ConsumeType efxType;
    public int consumeValue;
    public int consumeStayTime;

    public int level;
    public int possableCount;
    public int category;
    public Define.EquipType wearType;

    public Stat limitStat;

    public Stat baseStat;
    public AdditionalStat baseAdditional;

    public Stat addAbility;
    public AdditionalStat addAdditional;

    public List<List<VisualElement>> itemList;

    VisualElement buff;
    VisualElement heal;

    [MenuItem("Tools/Creat Item(Scriptable)")]
    public static void ShowMyEditor()
    {
        EditorWindow wnd = GetWindow<CustomItem>();
        wnd.titleContent = new GUIContent("Creat Item(Scriptable)");

        wnd.minSize = new Vector2(450, 200);
        wnd.maxSize = new Vector2(1920, 720);
    }
    public CustomItem()
    {
        itemList = new(Utils.GetEnumArray<Define.ItemType>().Length);
        for (int i = 0; i < itemList.Capacity; i++)
            itemList.Add(new());
    }

    public void CreateGUI()
    {
        // 프로젝트의 모든 스프라이트 목록 가져오기
        var allObjectGuids = AssetDatabase.FindAssets("t:Sprite", spriePath);
        var allObjects = new List<Sprite>();
        foreach (var guid in allObjectGuids)
        {
            allObjects.Add(AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid)));
        }
        VisualElement root = rootVisualElement;

        CreatEctData(root);
        CreatConsumeData(root);
        CreatEquipData(root);

        Button button = new Button(() => { MakeItem(); })
        {
            text = "MakeItem"
        };
        root.Add(button);
        // 왼쪽 창이 고정된 두 창 보기 만들기
        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

        // 뷰를 루트 요소에 자식으로 추가하여 시각적 트리에 추가합니다
        root.Add(splitView);

        // TwoPaneSplitView에는 항상 두 개의 자식 요소가 필요합니다
        var leftPane = new ListView();
        splitView.Add(leftPane);
        m_RightPane = new VisualElement();
        splitView.Add(m_RightPane);

       
        // 모든 스프라이트의 이름으로 목록 보기 초기화
        //leftPane.makeItem = () => new Label();
        //leftPane.bindItem = (item, index) => { (item as Label).text = allObjects[index].name; };
        //leftPane.itemsSource = allObjects;

        //leftPane.selectionChanged += OnSpriteSelectionChange;
    }

    private void CreatEctData(VisualElement root)
    {
        AddTextElement(root, ItemType.Non, "ItemName", true)
            .RegisterValueChangedCallback(evt =>
        { itemName = evt.newValue; });


        AddEnumElement(root, ItemType.Non, "ItemType", itemType)
           .RegisterValueChangedCallback(evt =>
           { ChangeItemType((Define.ItemType)evt.newValue); });

        AddTextElement(root, ItemType.Ect, "Infomation" , true, true)
            .RegisterValueChangedCallback(evt =>
        { itemInfo = evt.newValue; });
    }
    private void CreatConsumeData(VisualElement root)
    {
        AddEnumElement(root, ItemType.Consume, "HealType", addType)
           .RegisterValueChangedCallback(evt =>
           { addType = (Define.HealType)evt.newValue; });

        AddEnumElement(root, ItemType.Consume, "EffectType", efxType)
            .RegisterValueChangedCallback(evt =>
            {
                efxType = (Define.ConsumeType)evt.newValue;
                buff.style.display =
                efxType == ConsumeType.During ?
                DisplayStyle.Flex : DisplayStyle.None;
            });


        AddIntegerElement(root, ItemType.Consume, "ConsumeValue")
            .RegisterValueChangedCallback(evt =>
            { this.consumeValue = evt.newValue; });

        IntegerField stay = AddIntegerElement(root, ItemType.Consume, "stayTimeValue");
        stay.RegisterValueChangedCallback(evt =>
        { this.consumeStayTime = evt.newValue; });
        buff = stay;

    }

    private void CreatEquipData(VisualElement root)
    {

        AddIntegerElement(root, ItemType.Equip, "Level")
        .RegisterValueChangedCallback(evt =>
        { this.level = evt.newValue; });


        AddIntegerElement(root, ItemType.Equip, "Category")
        .RegisterValueChangedCallback(evt =>
        { this.category = evt.newValue; });

        AddIntegerElement(root, ItemType.Equip, "PossableCount")
        .RegisterValueChangedCallback(evt =>
        { this.possableCount = evt.newValue; });

        AddEnumElement(root, ItemType.Equip, "WearType", wearType)
           .RegisterValueChangedCallback(evt =>
           { wearType = (Define.EquipType)evt.newValue; });


        AddLabel(root, "Limit_Stat", FontStyle.Bold, 20);



        AddIntegerElement(root, ItemType.Equip, "LimitStr")
        .RegisterValueChangedCallback(evt =>
        { this.limitStat.str = evt.newValue; });


        AddIntegerElement(root, ItemType.Equip, "LimitDef")
        .RegisterValueChangedCallback(evt =>
        { this.limitStat.def = evt.newValue; });

        AddIntegerElement(root, ItemType.Equip, "LimitMan")
        .RegisterValueChangedCallback(evt =>
        { this.limitStat.man = evt.newValue; });


        AddIntegerElement(root, ItemType.Equip, "LimitLuk")
        .RegisterValueChangedCallback(evt =>
        { this.limitStat.luk = evt.newValue; });




        AddLabel(root, "baseAbility", FontStyle.Bold, 20);



        AddIntegerElement(root, ItemType.Equip, "Str")
        .RegisterValueChangedCallback(evt =>
        { this.baseStat.str = evt.newValue; });


        AddIntegerElement(root, ItemType.Equip, "Def")
        .RegisterValueChangedCallback(evt =>
        { this.baseStat.def = evt.newValue; });

        AddIntegerElement(root, ItemType.Equip, "Man")
        .RegisterValueChangedCallback(evt =>
        { this.baseStat.man = evt.newValue; });

        AddIntegerElement(root, ItemType.Equip, "Luk")
        .RegisterValueChangedCallback(evt =>
        { this.baseStat.luk = evt.newValue; });



        AddIntegerElement(root, ItemType.Equip, "Power")
        .RegisterValueChangedCallback(evt =>
        { this.baseAdditional.power = evt.newValue; });


        AddIntegerElement(root, ItemType.Equip, "Magic")
        .RegisterValueChangedCallback(evt =>
        { this.baseAdditional.magic = evt.newValue; });

        AddIntegerElement(root, ItemType.Equip, "Defence")
          .RegisterValueChangedCallback(evt =>
          { this.baseAdditional.defence = evt.newValue; });


        AddIntegerElement(root, ItemType.Equip, "Speed")
           .RegisterValueChangedCallback(evt =>
           { this.baseAdditional.speed = evt.newValue; });




        AddLabel(root, "AddAbility", FontStyle.Bold, 20);





        AddIntegerElement(root, ItemType.Equip, "AddStr")
        .RegisterValueChangedCallback(evt =>
        { this.addAbility.str = evt.newValue; });


        AddIntegerElement(root, ItemType.Equip, "AddDef")
        .RegisterValueChangedCallback(evt =>
        { this.addAbility.def = evt.newValue; });

        AddIntegerElement(root, ItemType.Equip, "AddMan")
        .RegisterValueChangedCallback(evt =>
        { this.addAbility.man = evt.newValue; });

        AddIntegerElement(root, ItemType.Equip, "AddLuk")
        .RegisterValueChangedCallback(evt =>
        { this.addAbility.luk = evt.newValue; });

        AddIntegerElement(root, ItemType.Equip, "AddPower")
        .RegisterValueChangedCallback(evt =>
        { this.addAdditional.power = evt.newValue; });


        AddIntegerElement(root, ItemType.Equip, "AddMagic")
        .RegisterValueChangedCallback(evt =>
        { this.addAdditional.magic = evt.newValue; });

        AddIntegerElement(root, ItemType.Equip, "AddDefence")
          .RegisterValueChangedCallback(evt =>
          { this.addAdditional.defence = evt.newValue; });


        AddIntegerElement(root, ItemType.Equip, "AddSpeed")
           .RegisterValueChangedCallback(evt =>
           { this.addAdditional.speed = evt.newValue; });
    }

    
    void AddLabel(VisualElement root, string labelName, FontStyle fontStyle, float fontSize = -1)
    {
        Label label = new Label(labelName);
        label.style.unityFontStyleAndWeight = fontStyle;
        if (fontSize > 0)
        {
            label.style.fontSize = fontSize;
        }
        root.Add(label);
    }

    IntegerField AddIntegerElement(VisualElement root, ItemType _itemType, string labelName, bool display = true,int maxValue = int.MaxValue)
    {
        IntegerField field = new(labelName, maxValue);
        AddVisualElement(GetItemList(_itemType), root, field);
        field.style.display = display && (_itemType == ItemType.Equip || _itemType == ItemType.Non) ?
            DisplayStyle.Flex : DisplayStyle.None;

        return field;
    }

    EnumField AddEnumElement<T>(VisualElement root, ItemType _itemType, string labelName, T type, bool display = true) where T : System.Enum
    {
        EnumField field = new EnumField(labelName, type);
        AddVisualElement(GetItemList(_itemType), root, field);
        field.style.display = display && (_itemType == ItemType.Equip || _itemType == ItemType.Non)? 
            DisplayStyle.Flex : DisplayStyle.None;
        
        return field;
    }

    TextField AddTextElement(VisualElement root, ItemType _itemType, string labelName, bool display = true, bool? multi = null)
    {
        TextField field = new TextField(labelName);
        field.style.display = display && (_itemType == ItemType.Equip || _itemType == ItemType.Non) ?
            DisplayStyle.Flex : DisplayStyle.None;

        if (multi.HasValue)
        {
            field.multiline = multi.Value;
        }
        AddVisualElement(GetItemList(_itemType), root, field);
        return field;
    }


    void AddVisualElement(List<VisualElement> typeList, VisualElement root, VisualElement visualElement)
    {
        if(root != null)
            root.Add(visualElement);
        if(typeList != null)
            typeList.Add(visualElement);
    }
    List<VisualElement> GetItemList(ItemType _itemType)
    {
        if (_itemType == ItemType.Non)
            return null;
        return itemList[(int)_itemType];
    }


 


    private void ChangeItemType(ItemType _itemType)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            DisplayStyle state = i == (int)_itemType ? DisplayStyle.Flex : DisplayStyle.None;
            List<VisualElement> list = itemList[i];
            for (int j = 0; j < list.Count; j++)
            {
                list[j].style.display = state;
            }
        }
        if(_itemType == ItemType.Consume)
        {
            buff.style.display = efxType == ConsumeType.During ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }


    private void OnSpriteSelectionChange(IEnumerable<object> selectedItems)
    {
        // Clear all previous content from the pane
        m_RightPane.Clear();

        // Get the selected sprite
        var selectedSprite = selectedItems.First() as Sprite;
        if (selectedSprite == null)
            return;

        // Add a new Image control and display the sprite
        var spriteImage = new Image();
        spriteImage.scaleMode = ScaleMode.ScaleToFit;
        spriteImage.sprite = selectedSprite;

        // Add the Image control to the right-hand pane
        m_RightPane.Add(spriteImage);
    }

    private void MakeItem()
    {

    }
}
