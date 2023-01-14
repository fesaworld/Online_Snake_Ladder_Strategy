using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;

public class CardManager : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public GameObject PlayerPointbarObject;
    public GameObject GoCard;
    public SpriteRenderer CardInfoSpriteRenderer;
    public Button CardLevel1_Button, CardLevel2_Button, CardLevel3_Button, CardOnHandLeft_Button, CardOnHandMiddle_Button, CardOnHandRight_Button, PutButton, SellButton, SaveButton;
    public Image CardLevel1_Image, CardLevel2_Image, CardLevel3_Image;
    public Sprite CardLevel1BtnActive_Sprite, CardLevel1BtnInactive_Sprite, CardLevel2BtnActive_Sprite, CardLevel2BtnInactive_Sprite, CardLevel3BtnActive_Sprite, CardLevel3BtnInactive_Sprite;

    public Card CardOnHand;
    public List<Card> SavedCards;
    public List<Queue<Card>> CardPile = new List<Queue<Card>>();

    public Item ItemOnHand;
    public List<Item> ItemOnBoard;
    public Item UsedItem;

    [HideInInspector]
    public bool IsGotPlayerPointbarObject = false;
    public bool IsReadyPutItem = false;
    public static bool ClickMousePlaceItem;

    public string BoughtCardLevel;
    public int BoughtCardIndex;

    GameplayManager gameplayManager;
    PointBar pointBar;
    GameObject goCurrentItemPlaced;
    GameObject tempItemObjectGambar;
    SpriteRenderer tempItemObjectGambarRenderer;
    string IsPlayerPlacedItem = "not yet";
    string conditionPutItem;
    public bool IsPlayerBuyCard = false;

    // Start is called before the first frame update
    void Start()
    {
        //gameplayManager = GameObject.Find("GameManager").GetComponent<GameplayManager>();
        //cardInfoAndButtonVisibility(false, false);

        //CardOnHandLeft_Button.interactable = false;
        //CardOnHandMiddle_Button.interactable = false;
        //CardOnHandRight_Button.interactable = false;

        //if(PhotonNetwork.IsMasterClient)
        //{
        //    CardPile.Add(CardPileLevel(Card.CardLevel.One));
        //    CardPile.Add(CardPileLevel(Card.CardLevel.Two));
        //    CardPile.Add(CardPileLevel(Card.CardLevel.Three));

        //    object[] cardPilesData = new object[] { CardPile[0].ToArray(), CardPile[1].ToArray(), CardPile[2].ToArray() };

        //    PhotonNetwork.RaiseEvent(RaiseEventCode.SEND_PILE_OF_CARDS_DATA, cardPilesData, RaiseEventOptions.Default, SendOptions.SendReliable);
        //}

        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPointbarObject != null && IsGotPlayerPointbarObject == false) {
            pointBar = PlayerPointbarObject.GetComponent<PointBar>();
            IsGotPlayerPointbarObject = true;
        }

        if(IsReadyPutItem) {
            PutItemOnBoard();
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(3f);

        gameplayManager = GameObject.Find("GameManager").GetComponent<GameplayManager>();
        cardInfoAndButtonVisibility(false, false);

        CardOnHandLeft_Button.interactable = false;
        CardOnHandMiddle_Button.interactable = false;
        CardOnHandRight_Button.interactable = false;

        if (PhotonNetwork.IsMasterClient)
        {
            CardPile.Add(CardPileLevel(Card.CardLevel.One));
            CardPile.Add(CardPileLevel(Card.CardLevel.Two));
            CardPile.Add(CardPileLevel(Card.CardLevel.Three));

            object[] cardPilesData = new object[] { CardPile[0].ToArray(), CardPile[1].ToArray(), CardPile[2].ToArray() };

            PhotonNetwork.RaiseEvent(RaiseEventCode.SEND_PILE_OF_CARDS_DATA, cardPilesData, RaiseEventOptions.Default, SendOptions.SendReliable);
        }


    }

    //=========================Bagian Kartu=============================

    //Method yang digunakan untuk membuat tumpukkan kartu (start)
    public List<Card> CreatePileOfCards() {
        List<Card> cards = new List<Card>();
        for(int i = 1; i <= 9; i++) {
            for(int cloneCard = 0; cloneCard < 2; cloneCard++) {
                if(i <= 6) {
                    cards.Add(new Card(i, Card.CardLevel.One, Card.CardType.Snake));
                    cards.Add(new Card(i, Card.CardLevel.One, Card.CardType.Ladder));
                } if(i <= 3) {
                    cards.Add(new Card(i, Card.CardLevel.Two, Card.CardType.Snake));
                    cards.Add(new Card(i, Card.CardLevel.Two, Card.CardType.Ladder));
                }

                //Belum ada asset kartunya 
                // if(i == 1) {
                //     cards.Add(new Card(i, Card.CardLevel.Two, Card.CardType.Snake2Head));
                //     cards.Add(new Card(i, Card.CardLevel.Two, Card.CardType.Ladder2Tail));
                // }
            }

            if(i <= 6) {
                cards.Add(new Card(i, Card.CardLevel.One, Card.CardType.Snake));
                cards.Add(new Card(i, Card.CardLevel.One, Card.CardType.Ladder));
            }
            if(i <= 3) {
                cards.Add(new Card(i, Card.CardLevel.Two, Card.CardType.Snake));
                cards.Add(new Card(i, Card.CardLevel.Two, Card.CardType.Ladder));
            }
            if(i >= 4 && i <= 5) {
                cards.Add(new Card(i, Card.CardLevel.Two, Card.CardType.Snake));
                cards.Add(new Card(i, Card.CardLevel.Two, Card.CardType.Ladder));
            }

            cards.Add(new Card(i, Card.CardLevel.Three, Card.CardType.Snake));
            cards.Add(new Card(i, Card.CardLevel.Three, Card.CardType.Ladder));
        }

        return cards;
    }
    
    public Queue<Card> CardPileLevel(Card.CardLevel level) {
        Queue<Card> cards = new Queue<Card>();
        foreach(Card card in CreatePileOfCards()) {
            if(card.Level == level) {
                cards.Enqueue(card);
            }
        }

        return ShuffleCard(cards);
    }

    public Queue<Card> ShuffleCard(Queue<Card> cards) {
        Card[] cardList = cards.ToArray();
        System.Random random = new System.Random();

        for(int n = cardList.Length - 1; n > 0; --n) {
            int k = random.Next(n + 1);
            Card temp = cardList[n];
            cardList[n] = cardList[k];
            cardList[k] = temp;
        }

        Queue<Card> toQueueCards = new Queue<Card>();
        foreach(Card card in cardList) {
            toQueueCards.Enqueue(card);
        }

        return toQueueCards;
    }

    void getPileOfCardsFromMasterClient(Card[] cardsLvl1, Card[] cardsLvl2, Card[] cardsLvl3) {
        Queue<Card> cardLvl1Queue = new Queue<Card>();
        Queue<Card> cardLvl2Queue = new Queue<Card>();
        Queue<Card> cardLvl3Queue = new Queue<Card>();

        if(cardsLvl1 != null){
            foreach(Card card in cardsLvl1) {
                cardLvl1Queue.Enqueue(card);
            }

            CardPile.Add(cardLvl1Queue);
        }
        if(cardsLvl2 != null){
            foreach(Card card in cardsLvl2) {
                cardLvl2Queue.Enqueue(card);
            }

            CardPile.Add(cardLvl2Queue);
        }
        if(cardsLvl3 != null){
            foreach(Card card in cardsLvl3) {
                cardLvl3Queue.Enqueue(card);
            }

            CardPile.Add(cardLvl3Queue);
        }
    }

    //Method yang digunakan untuk membuat tumpukkan kartu (end)

    public void SetCardSpriteAndInteractable(string cardLevel, bool cardIsActive) {
        if(cardLevel.Equals("1")) {
            if(cardIsActive) {
                CardLevel1_Button.interactable = true;
                CardLevel1_Image.sprite = CardLevel1BtnActive_Sprite;
            } else {
                CardLevel1_Button.interactable = false;
                CardLevel1_Image.sprite = CardLevel1BtnInactive_Sprite;
            }  
        } else if(cardLevel.Equals("2")) {
            if(cardIsActive) {
                CardLevel2_Button.interactable = true;
                CardLevel2_Image.sprite = CardLevel2BtnActive_Sprite;
            } else {
                CardLevel2_Button.interactable = false;
                CardLevel2_Image.sprite = CardLevel2BtnInactive_Sprite;
            }
        } else if(cardLevel.Equals("3")) {
            if(cardIsActive) {
                CardLevel3_Button.interactable = true;
                CardLevel3_Image.sprite = CardLevel3BtnActive_Sprite;
            } else {
                CardLevel3_Button.interactable = false;
                CardLevel3_Image.sprite = CardLevel3BtnInactive_Sprite;
            }
        }
    }

    public void BuyCardButton() {
        gameplayManager.EndTurnButton.interactable = false;
        //Disini tambahin soundeffect beli kartu
        string cardLevel = EventSystem.current.currentSelectedGameObject.name.ToString();
        switch (cardLevel)
        {
            case "CardLevel1_Button":
                BuyCard(7, CardPile[0]);
                break;
            case "CardLevel2_Button":
                BuyCard(10, CardPile[1]);
                break;
            case "CardLevel3_Button":
                BuyCard(13, CardPile[2]);
                break;
        }

        object[] buyCardData = new object[] { PlayerScript.PlayerNickname, this.CardOnHand };

        PhotonNetwork.RaiseEvent(RaiseEventCode.SHOW_BOUGHT_CARD_NOTIFICATION, buyCardData, RaiseEventOptions.Default, SendOptions.SendReliable);

        cardBoughtNotification(PlayerScript.PlayerNickname, this.CardOnHand);

        if(SavedCards.Count < 3) {
            SaveButton.interactable = true;
        } else  {
            SaveButton.interactable = false;
        }

        this.CardOnHand.DisplayCard(GoCard);

        PutButton.interactable = true;
        SellButton.interactable = true;

        IsPlayerBuyCard = false;
    }

    public void SaveCardButton() {
        //SoundManager.PlaySoundEffect("ButtonClick");
        SaveCard();
        gameplayManager.EndTurnButton.interactable = true;
    }

    public void PutItemButton() {
        //SoundManager.PlaySoundEffect("ButtonClick");
        cardInfoAndButtonVisibility(false, true);
        IsReadyPutItem = true;
    }

    public void SellCardButton() {
        //SoundManager.PlaySoundEffect("ButtonClick");
        Card.CardLevel cardLevel = CardOnHand.Level;
        switch (cardLevel)
        {
            case Card.CardLevel.One:
                SellCard(4, CardPile[0]);
                break;  
            case Card.CardLevel.Two:
                SellCard(6, CardPile[1]);
                break;
            case Card.CardLevel.Three:
                SellCard(8, CardPile[2]);
                break;
        }
        gameplayManager.EndTurnButton.interactable = true;
    }

    public void BuyCard(int cardPrice, Queue<Card> cards) {
        pointBar.SubtractPlayerPointValue(cardPrice);
        this.CardOnHand = cards.Dequeue();
    }

    public void cardBoughtNotification(string paramBuyerName, Card boughtCard) {
        string buyerName;
        if(paramBuyerName != null) {
            buyerName = paramBuyerName;
        } else {
            buyerName = "Unknown";
        }

        //SoundManager.PlaySoundEffect("BuyCard");

        switch(boughtCard.Level)
        {
            case Card.CardLevel.One:
                StartCoroutine(gameplayManager.DisplayNotificationInfo(buyerName + " membeli kartu level 1", 2f, false));
                break;
            case Card.CardLevel.Two:
                StartCoroutine(gameplayManager.DisplayNotificationInfo(buyerName + " membeli kartu level 2", 2f, false));
                break;
            case Card.CardLevel.Three:
                StartCoroutine(gameplayManager.DisplayNotificationInfo(buyerName + " membeli kartu level 3", 2f, false));
                break;
            default:
                break;
        }

        if(buyerName != PlayerScript.PlayerNickname)
        {
            removeCardFromCardPile(boughtCard);
        }
        else
        {
            Debug.Log("This player bought a card!");
        }
    }

    public void SaveCard() {
        this.SavedCards.Add(this.CardOnHand);
        this.SavedCards[this.SavedCards.Count-1].Owner = PlayerScript.PlayerNickname;
        this.SavedCards[this.SavedCards.Count-1].DisplaySaveCard(this.SavedCards.Count, false);

        cardInfoAndButtonVisibility(false, false);
    }

    public void TakeCardFromSavedCard() {
        gameplayManager.EndTurnButton.interactable = false;
        int indexSaveCard = SavedCards.FindIndex(card => card.CardObject.name == EventSystem.current.currentSelectedGameObject.name.ToString());

        if (indexSaveCard >= 0)
        {
            Card cardTaked = SavedCards[indexSaveCard];
            
            //Di method ini tak tuker isinya terus kondisinya tak ganti pake nama spritenya si cardinfo
            if (CardInfoSpriteRenderer.sprite.name.ToString() == "0")
            {
                SavedCards[SavedCards.Count-1].DisplaySaveCard(SavedCards.Count, true);
                SavedCards.RemoveAt(indexSaveCard);
                if (SavedCards.Count > 0)
                {
                    for (int savedCard = indexSaveCard; savedCard < SavedCards.Count; savedCard++)
                    {
                        SavedCards[savedCard].DisplaySaveCard(savedCard+1, false);
                    }
                }
            } else
            {
                SavedCards[indexSaveCard] = CardOnHand;
                SavedCards[indexSaveCard].CardObject = cardTaked.CardObject;
                SavedCards[indexSaveCard].Owner = PlayerScript.PlayerNickname;
                SavedCards[indexSaveCard].DisplaySaveCard(indexSaveCard+1, false);
            }

            CardOnHand = cardTaked; // set card taked to on hand

            if(SavedCards.Count < 3) {
                SaveButton.interactable = true;
            } else  {
                SaveButton.interactable = false;
            }

            CardOnHand.DisplayCard(GoCard);

            PutButton.interactable = true;
            SellButton.interactable = true;
        }
    }

    public void SellCard(int cardPrice, Queue<Card> cards) {
        pointBar.AddPlayerPointValue(cardPrice);
        cards.Enqueue(this.CardOnHand);

        object[] sellCardData = new object[] { this.CardOnHand };

        PhotonNetwork.RaiseEvent(RaiseEventCode.PLAYER_SELL_CARD, sellCardData, RaiseEventOptions.Default, SendOptions.SendReliable);

        cardInfoAndButtonVisibility(false, false);
    }

    void removeCardFromCardPile(Card cardOut)
    {
        switch(cardOut.Level)
        {
            case Card.CardLevel.One:
                CardPile[0].Dequeue();
                break;
            case Card.CardLevel.Two:
                CardPile[1].Dequeue();
                break;
            case Card.CardLevel.Three:
                CardPile[2].Dequeue();
                break;
            default:
                break;
        }
    }

    void moveCardToTopOfCardPile(Card cardIn) {
        switch(cardIn.Level)
        {
            case Card.CardLevel.One:
                CardPile[0].Enqueue(cardIn);
                break;
            case Card.CardLevel.Two:
                CardPile[1].Enqueue(cardIn);
                break;
            case Card.CardLevel.Three:
                CardPile[2].Enqueue(cardIn);
                break;
            default:
                break;
        }
    }

    void cardInfoAndButtonVisibility(bool isActive, bool isPutItem) {
        if(!isActive && !isPutItem) {
            CardInfoSpriteRenderer.sprite = Resources.Load<Sprite>("New Images Asset/Card/0");
            if(SaveButton.interactable == true && PutButton.interactable == true && SellButton.interactable == true) {
                SaveButton.interactable = false;
                PutButton.interactable = false;
                SellButton.interactable = false;
            }
        } else if(!isActive && isPutItem) {
            SaveButton.interactable = false;
            PutButton.interactable = false;
            SellButton.interactable = false;
        }
    }

    //========================End Bagian Kartu=========================

    //=========================Bagian Item=============================

    public void PutItemOnBoard() {
        //Mengambil object kartu yang ada di tangan
        Card _card = CardOnHand;
        //mengambil path yang akan digunakan untuk mengakses prefabs item
        string _goName = _card.Level+"/"+_card.Type+"/"+_card.Value;
        //proses mengakses prefabs item yang akan dipasang
        goCurrentItemPlaced = Resources.Load("Items/"+_goName) as GameObject;
        //Mengisi variabel conditionPutItem dengan mengeksekusi method selectBoardToPutItem
        conditionPutItem = SelectBoardToPutItem(goCurrentItemPlaced, _card.Type, _card);

        if (conditionPutItem == "snake/ladder putted")
        {
            //Kondisi jika kembalian dari method isItemPlaced bernilai true
            if (IsItemPlaced() == true)
            {
                //Mengeksekusi method itemAttached
                itemAttached(conditionPutItem, _goName);

                object[] attachedCardData = new object[] { _card };

                PhotonNetwork.RaiseEvent(RaiseEventCode.PLAYER_ATTACH_ITEM_ON_BOARD, attachedCardData, RaiseEventOptions.Default, SendOptions.SendReliable);

                moveCardToTopOfCardPile(_card);
            }
        }
    }

    public string SelectBoardToPutItem(GameObject item, Card.CardType itemType, Card cardOnHand) {
        //deklarasi variabel condition
        string condition = "empty";

        //kondisi ketika meng-klik kiri pada mouse
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("press");
            //Mengambil nilai Vector3 dari posisi kursor mouse saat melakukan klik kiri
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Mengambil nilai Vector2 dari posisi kursor mouse saat melakukan klik kiri
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            //Mengambil nilai RaycastHit2D dari kursor mouse saat melakukan klik kiri
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            //Mengisi nilai dari condition dengan mengeksekusi method Checkboard dengan parameter RaycastHit2d, GameObject, ItemType dan CardOnHand
            condition = this.CheckBoard(hit, item, itemType, cardOnHand);
            //Memasukkan nilai dari variabel condition ke dalam properti isPlayerPlacedItem
            this.IsPlayerPlacedItem = condition;
        }
        //kondisi ketika properti isPlayerPlaced memiliki nilai "snake/ladder putted"
        else if (this.IsPlayerPlacedItem == "snake/ladder putted")
        {
            //Mengisi nilai dari condition dengan memberikan nilai dari properti isPlayerPlacedItem
            condition = this.IsPlayerPlacedItem;
        }

        //Kondisi ketika melepas klik kiri dari mouse
        if (Input.GetMouseButtonUp(0))
        {
            //mengisi parameter clickMousePlaceItem dengan nilai false
            ClickMousePlaceItem = false;
        }

        //memberikan nilai kembalian berupa variabel condition
        return condition;
    }

    public string CheckBoard(RaycastHit2D hit, GameObject item, Card.CardType itemType, Card cardOnHand) {
        //kondisi ketika collider pada raycasthit2d tidak kosong atau tidak bernilai null
        if (hit.collider != null)
        {
            string objectTagCollidedByItem = hit.collider.gameObject.tag;
            GameObject gambarObject, parentObject;         
            switch(objectTagCollidedByItem)
            {
                case "Petak-100":
                    //SoundManager.PlaySoundEffect("WrongPlace");
                    StartCoroutine(gameplayManager.DisplayNotificationInfo("Dilarang memasang item di petak 100!", 2f, false));
                    return "error";
                case "kepala":
                    gambarObject = hit.collider.gameObject.transform.GetChild(0).gameObject;
                    if(gambarObject.activeSelf.Equals(false))
                    {
                        //SoundManager.PlaySoundEffect("WrongPlace");
                        StartCoroutine(gameplayManager.DisplayNotificationInfo(CheckTailTrigger.AlertMessage, 2f, false));
                    }
                    else
                    {
                        //SoundManager.PlaySoundEffect("WrongPlace");
                        StartCoroutine(gameplayManager.DisplayNotificationInfo("Dilarang memasang disini, ada item lain!", 2f, false));
                    }
                    return "error";
                case "ekor":
                    parentObject = hit.collider.gameObject.transform.parent.gameObject;
                    gambarObject = parentObject.transform.GetChild(0).gameObject;
                    if(gambarObject.activeSelf.Equals(false))
                    {
                        //SoundManager.PlaySoundEffect("WrongPlace");
                        StartCoroutine(gameplayManager.DisplayNotificationInfo(CheckTailTrigger.AlertMessage, 2f, false));
                    }
                    else
                    {
                        //SoundManager.PlaySoundEffect("WrongPlace");
                        StartCoroutine(gameplayManager.DisplayNotificationInfo("Dilarang memasang disini, ada item lain!", 2f, false));
                    }
                    return "error";
                case "Player":
                    //SoundManager.PlaySoundEffect("WrongPlace");
                    StartCoroutine(gameplayManager.DisplayNotificationInfo("Dilarang memasang disini, ada pion pemain!", 2f, false));
                    return "error";
                case "Barrier":
                    //SoundManager.PlaySoundEffect("WrongPlace");
                    StartCoroutine(gameplayManager.DisplayNotificationInfo("Tidak boleh memasang diluar area papan!", 2f, false));
                    return "error";
                default:
                    return this.PasangItem(hit, item, itemType, cardOnHand);
            }
        }
        else
        {
            //SoundManager.PlaySoundEffect("WrongPlace");
            StartCoroutine(gameplayManager.DisplayNotificationInfo("Tidak boleh memasang diluar area papan!", 2f, false));
            return "error";
        }
    }

    public string PasangItem(RaycastHit2D hit, GameObject item, Card.CardType itemType, Card cardOnHand) {
        int[] boardPosItemPlaced;
        Vector3 itemPosition = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);

        if (hit.collider.gameObject.name == "Waypoint")
        {
            boardPosItemPlaced = new int[]{1};
        }
        else
        {
            string resultString = Regex.Match(hit.collider.gameObject.name, @"\d+").Value;
            int clickPosisi = int.Parse(resultString) + 1;
            if (itemType == Card.CardType.Ladder2Tail || itemType == Card.CardType.Snake2Head)
            {
                boardPosItemPlaced = new int[]{clickPosisi-1, clickPosisi+1};                
            } else
            {
                boardPosItemPlaced = new int[]{clickPosisi};
            }
        }

        // DICOMMENT KARENA TESTING
        if (GameObject.Find(item.name+"(Clone)") != null)
        {
            PhotonNetwork.Destroy(GameObject.Find(item.name+"(Clone)"));
        }

        string itemPrefabPath = "Items/" + cardOnHand.Level.ToString() + "/" + itemType.ToString() + "/" + item.name;
        this.ItemOnHand = new Item{
            Owner = PlayerScript.PlayerNickname,
            Age = 0,
            BoardPosition = boardPosItemPlaced[0],
            Type = itemType,
            ItemObject = PhotonNetwork.Instantiate(itemPrefabPath, itemPosition, item.transform.rotation),
        };

        if (!this.ItemOnHand.ItemObject.activeSelf)
        {
            this.ItemOnHand.ItemObject.SetActive(true);
        }

        // DICOMMENT KARENA TESTING
        if (CheckTailTrigger.TriggerByTail)
        {
            CheckTailTrigger.TriggerByTail = false;
        }
        
        this.ItemOnHand.ItemObject.transform.Find("gambar").gameObject.SetActive(false);
        this.ItemOnHand.Spawn(hit.collider.gameObject.transform.position);
        ClickMousePlaceItem = true;
        return "snake/ladder putted";
    }

    public bool IsItemPlaced() {
        bool isItemPlaced = false;
        if (CheckTailTrigger.TailChecked && CheckTailTrigger.AttachedItemName == null) // jika ekor tidak menyentuh object lain
        {
            isItemPlaced = true;
            CheckTailTrigger.TailChecked = false;
        }
        else if (CheckTailTrigger.TailChecked && CheckTailTrigger.AttachedItemName != null)
        {
            GameObject item = GameObject.Find(CheckTailTrigger.AttachedItemName);
            if (item != null && !CheckTailTrigger.TriggerByTail) // jika ekor menyentuh object lain tetapi setelah itu dipasang kembali dan ekor tidak mengenai object lain
            {
                isItemPlaced = true;
                CheckTailTrigger.AttachedItemName = null;
                CheckTailTrigger.TailChecked = false;
            }
            CheckTailTrigger.TailChecked = false;
        }
        return isItemPlaced;
    }

    void itemAttached(string condition, string _goName) {
        //Kondisi jika variabel condition bernilai "snake/ladder putted"
        if (condition == "snake/ladder putted")
        {
            //kondisi ketika ada GameObject yang namanya sama dengan isi dari variabel _goName
            if (GameObject.Find(_goName) != null)
            {
                //mengganti nama itemObject yang berada di itemOnHand dengan nilai dari _goName + "_2"
                this.ItemOnHand.ItemObject.name = _goName+"_2";
            }
            //kondisi ketika tidak ada GameObject yang namanya sama dengan isi dari variabel _goName
            else
            {
                //mengganti nama itemObject yang berada di itemOnHand dengan nilai dari _goName
                this.ItemOnHand.ItemObject.name = _goName;
            }

            tempItemObjectGambar = this.ItemOnHand.ItemObject.transform.Find("gambar").gameObject;
            
            changeAttachedItemSprite(tempItemObjectGambar, _goName, PlayerScript.PlayerTurn);

            //Mengaktifkan GameObject gambar yang ada di itemOnHand
            this.ItemOnHand.ItemObject.transform.Find("gambar").gameObject.SetActive(true);

            //Memberikan nilai false pada properti clickMousePlaceItem
            ClickMousePlaceItem = false;

            string itemObjectName = Regex.Match(_goName, @"\d+").Value;

            object[] attachedItemData = new object[] { this.ItemOnHand, itemObjectName, PlayerScript.PlayerTurn, _goName };

            PhotonNetwork.RaiseEvent(RaiseEventCode.SEND_ATTACHED_ITEM_DATA, attachedItemData, RaiseEventOptions.Default, SendOptions.SendReliable);

            //Menambahkan objek ItemOnHand ke dalam list ItemOnBoard
            ItemOnBoard.Add(this.ItemOnHand);

            //Selama nilai dari player kurang dari playerinOverlap.Count, jalankan...
            // for (int player = 0; player < playerinOverlap.Count; player++)
            // {
            //     //Menonaktifkan collider pada player yang berada di dalam kotak yang sama
            //     playerinOverlap[player].pawn.GetComponent<Collider2D>().enabled = false;
            // }

            // //Kosongkan list playerinOverlap
            // playerinOverlap.Clear();

            //Kondisi jika itemOnHand pada pemain bertipe Snake atau Snake2Head
            if (ItemOnHand.Type == Card.CardType.Snake || ItemOnHand.Type == Card.CardType.Snake2Head)
            {
                //SoundManager.PlaySoundEffect("AttachSnake");
            }
            //Kondisi jika itemOnHand pada pemain bertipe selain Snake atau Snake2Head
            else
            {
                //SoundManager.PlaySoundEffect("AttachLadder");
            }
        }

        //Mengganti nilai dari isReadyPutItem menjadi false
        IsReadyPutItem = false;
        //Mengganti nilai dari conditionPutItem menjadi "empty"
        conditionPutItem = "empty";
        cardInfoAndButtonVisibility(false, false);
        gameplayManager.EndTurnButton.interactable = true;
    }

    void changeAttachedItemSprite(GameObject itemObjectGambar, string itemName, int ownerTurn) {
        tempItemObjectGambarRenderer = itemObjectGambar.GetComponent<SpriteRenderer>();
        switch(ownerTurn) {
            case 1:
                break;
            case 2:
                tempItemObjectGambarRenderer.sprite = Resources.Load<Sprite>("New Images Asset/Items/P2/"+ itemName);
                break;
            case 3:
                tempItemObjectGambarRenderer.sprite = Resources.Load<Sprite>("New Images Asset/Items/P3/"+ itemName);
                break;
            case 4:
                tempItemObjectGambarRenderer.sprite = Resources.Load<Sprite>("New Images Asset/Items/P4/"+ itemName);
                break;
        }
    }

    void getItemOnBoardData(Item attachedItemObj, string attachedItemPrefabName, int attachedItemOwnerTurn, string attachedItemName) {
        GameObject tempItemGameObject = GameObject.Find(attachedItemPrefabName + "(Clone)");
        Transform tempItemDestination = tempItemGameObject.transform.Find("ekor");

        if(attachedItemObj.Type.Equals(Card.CardType.Snake) || attachedItemObj.Type.Equals(Card.CardType.Snake2Head))
        {
            //SoundManager.PlaySoundEffect("AttachSnake");
        }
        else
        {
            //SoundManager.PlaySoundEffect("AttachLadder");
        }

        tempItemObjectGambar = tempItemGameObject.transform.Find("gambar").gameObject;
        changeAttachedItemSprite(tempItemObjectGambar, attachedItemName, attachedItemOwnerTurn);
        
        Item attachedItem = new Item{
            Owner = attachedItemObj.Owner,
            Age = attachedItemObj.Age,
            BoardPosition = attachedItemObj.BoardPosition,
            Type = attachedItemObj.Type,
            Destination = tempItemDestination,
            ItemObject = tempItemGameObject
        };

        ItemOnBoard.Add(attachedItem);
    }

    public void DestroyUsedItem(Item destroyedItem) {
        // if(destroyedItem.Type == Card.CardType.Snake || destroyedItem.Type == Card.CardType.Snake2Head)
        //     SoundManager.PlaySoundEffect("SnakeDied");
        // else
        //     SoundManager.PlaySoundEffect("LadderDestroyed");

        ItemOnBoard.Remove(destroyedItem);
    }

    //=========================End Bagian Item=========================
    new void OnEnable() {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    new void OnDisable() {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    public void NetworkingClient_EventReceived(EventData obj) {
        if(obj.Code == RaiseEventCode.SHOW_BOUGHT_CARD_NOTIFICATION)
        {
            object[] boughtCardData = (object[])obj.CustomData;

            string tempBuyerNickname = (string)boughtCardData[0];
            Card tempBoughtCard = (Card)boughtCardData[1];

            cardBoughtNotification(tempBuyerNickname, tempBoughtCard);
        }
        else if(obj.Code == RaiseEventCode.PLAYER_SELL_CARD)
        {
            object[] soldCardData = (object[])obj.CustomData;

            Card tempSoldCard = (Card)soldCardData[0];

            moveCardToTopOfCardPile(tempSoldCard);
        }
        else if(obj.Code == RaiseEventCode.PLAYER_ATTACH_ITEM_ON_BOARD)
        {
            object[] attachedCardData = (object[])obj.CustomData;

            Card tempAttachedCard = (Card)attachedCardData[0];

            moveCardToTopOfCardPile(tempAttachedCard);
        }
        else if(obj.Code == RaiseEventCode.SEND_ATTACHED_ITEM_DATA)
        {
            object[] attachedItemData = (object[])obj.CustomData;

            Item attachedItem = (Item)attachedItemData[0];
            string attachedItemPrefabName = (string)attachedItemData[1];
            int attachedItemOwnerTurn = (int)attachedItemData[2];
            string attachedItemName = (string)attachedItemData[3];

            getItemOnBoardData(attachedItem, attachedItemPrefabName, attachedItemOwnerTurn, attachedItemName);
        }
        else if(obj.Code == RaiseEventCode.SEND_PILE_OF_CARDS_DATA)
        {
            object[] cardPileData = (object[])obj.CustomData;

            Card[] cardLvl1Arr = (Card[])cardPileData[0];
            Card[] cardLvl2Arr = (Card[])cardPileData[1];
            Card[] cardLvl3Arr = (Card[])cardPileData[2];

            getPileOfCardsFromMasterClient(cardLvl1Arr, cardLvl2Arr, cardLvl3Arr);
            
        }
    }
}
