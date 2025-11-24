using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PuzzleGameManager : MonoBehaviour {

    enum STATE { wait, idle, touch, move, calc, finish, cancel }
    STATE state = STATE.wait;

    int sliceCnt;
    int imgNum;

    float tileScale;      // 기준크기 ÷ ImageSize;
    float tileSpan;       // 타일 간격
    float camScale;

    Transform origin;

    List<Sprite> sprites = new List<Sprite>();
    List<Transform> tiles = new List<Transform>();

    List<int> orders = new List<int>();
    List<int> moveTiles = new List<int>();
        
    int dir;              // 타일 이동방향 1:Up, 2:Right, 3:Down, 4:Left
    int tileNum;          // Click한 타일 번호
    bool canCalc = true;

    // Game UI
    GameObject panelQuit;
    GameObject panelComplete;
    Text txtTime;
    Text txtMove;

    int moveCnt = 0;
    float startTime;
    float timeSpan;

    bool canUI = true;

    // Use this for initialization
    void Awake () {
        InitGame();
        ShffulTile();
        DrawTiles();

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update () {
        switch (state) {
        case STATE.touch:
            CheckTiles();
            break;
        case STATE.move:
            MoveTiles();
            break;
        case STATE.calc:
            CalcOrder();
            break;
        case STATE.finish:
            SetFinish();
            break;
        case STATE.cancel:
            SetCancel();
            break;
        }

        if (canUI) SetTime();
        if (state != STATE.cancel && Input.GetKeyDown(KeyCode.Escape)) {
            state = STATE.cancel;
        } 
    }

    // Game UI <- Update
    void SetTime () {
        timeSpan = Time.time - startTime;
        int h = Mathf.FloorToInt(timeSpan / 3600);
        int m = Mathf.FloorToInt(timeSpan / 60 % 60);
        float s = timeSpan % 60;
        
        txtTime.text = string.Format("Time : {0:0}:{1:0}:{2:0.0}", h, m, s);
        txtMove.text = moveCnt.ToString("Move : 0");
    }

    // 게임 취소 <- Update
    void SetCancel() {
        state = STATE.wait;
        canUI = false;
        panelQuit.SetActive(true);
    }

    // DrawTile <- Update
    void DrawTiles () {
        state = STATE.wait;
        Transform parent = new GameObject("Tiles").transform;

        // 타일 간격 구하기
        Sprite sprite = tiles[0].GetComponent<SpriteRenderer>().sprite;
        tileSpan = sprite.bounds.size.x * tileScale;

        for (int y = 0; y < sliceCnt; y++) {
            for (int x = 0; x < sliceCnt; x++) {
                int idx = y * sliceCnt + x;

                // 타일의 index
                int n = orders[idx];
                if (n == -1) {
                    n = orders.Count - 1;
                }

                Vector3 pos = new Vector3(x * tileSpan, -y * tileSpan, 0);
                tiles[n].position = pos;
                tiles[n].parent = parent;
            }
        }

        state = STATE.idle;
    }

    // Check Tile <- Update
    void CheckTiles () {
        state = STATE.wait;

        // 이동 방향과 이동할 타일
        dir = 0;
        moveTiles.Clear();

        // 클릭한 타일과 공백 위치 찾기
        int tile = orders.FindIndex(x => x == tileNum);
        int blank = orders.FindIndex(x => x == -1);

        // 좌표 계산 (1D => 2D)
        int x1 = tile % sliceCnt;
        int y1 = tile / sliceCnt;

        int x2 = blank % sliceCnt;
        int y2 = blank / sliceCnt;

        // 세로 방향 조사
        if (x1 == x2) {
            // 공백 번호
            moveTiles.Add(blank);

            // 이동 방향과 행 간격
            dir = (y1 > y2) ? 1 : 3;
            int row = (y1 > y2) ? sliceCnt : -sliceCnt;
            int idx = blank + row;

            while (true) {
                moveTiles.Add(idx);
                idx += row;
                if ((dir == 1 && idx > tile) || (dir == 3 && idx < tile)) break;
            }
        }

        // 가로 방향 조사
        else if (y1 == y2) {
            moveTiles.Add(blank);

            // 이동 방향과 열간격
            dir = (x1 > x2) ? 4 : 2;
            int col = (x1 > x2) ? 1 : -1;
            int idx = blank + col;

            while (true) {
                moveTiles.Add(idx);
                idx += col;
                if ((dir == 2 && idx < tile) || (dir == 4 && idx > tile)) break;
            }
        }

        state = (moveTiles.Count > 0) ? STATE.move : STATE.idle;

        if (state == STATE.move) { 
            moveCnt += moveTiles.Count - 1;
        }
    }

    // MoveTiles <- Update
    void MoveTiles () {
        state = STATE.wait;

        // 타일의 이동 방향 Vector 
        Vector3[] vectors = {Vector3.zero, Vector3.up, Vector3.right, Vector3.down, Vector3.left};

        foreach (int idx in moveTiles) {
            int p = orders[idx];
            if (p == -1) continue;

            Vector3 pos = tiles[p].position;
            Vector3 target = pos + vectors[dir] * tileSpan;
            tiles[p].SendMessage("SetMove", target);
        }

        canCalc = true;
    }

    // 타일 색인 정리
    void CalcOrder () {
        if (!canCalc) {
            state = STATE.idle;
            return;
        }

        canCalc = false;
        state = STATE.wait;

        for (int i = 0; i < moveTiles.Count - 1; i++) {
            int n1 = moveTiles[i];
            int n2 = moveTiles[i + 1];
            orders[n1] = orders[n2];
        }

        // 공백 이동
        int blank = moveTiles[moveTiles.Count - 1];
        orders[blank] = -1;

        // 정리 완료인지 조사
        bool finished = true;
        for (int i = 0; i < orders.Count - 1; i++) {
            if (orders[i] != i) {
                finished = false;
                break;
            }
        }

        if (finished) {
            state = STATE.finish;
            canUI = false;
        } else {
            state = STATE.idle;
        }
    }

    // 정리 완료
    void SetFinish () {
        state = STATE.wait;

        foreach (Transform tile in tiles) {
            tile.GetComponent<SpriteRenderer>().material.SetInt("_count", 0);
        }

        // 마지막 타일
        int last = orders.Count - 1;
        tiles[last].gameObject.SetActive(true);
        tiles[last].position = tiles[last - 1].position + Vector3.right * tileSpan;

        origin.GetComponent<SpriteRenderer>().material.SetInt("_count", 1);

        // Complete Panel & Particle
        panelComplete.SetActive(true);

        GameObject star = Instantiate(Resources.Load("Star")) as GameObject;
        star.transform.position = new Vector3(5, -5, 0);

        // Fanfare
        if (!Settings.canMusic) return;

        AudioSource audioSrc = GetComponent<AudioSource>();
        audioSrc.loop = false;
        audioSrc.volume = 1;

        audioSrc.clip = Resources.Load("Audio/Fanfare") as AudioClip;
        audioSrc.Play();
    }

    // Shuffle <- Awake
    void ShffulTile () {
        //return;
        for (int i = 0; i < orders.Count - 1; i++) {
            int n = Random.Range(i + 1, orders.Count);
            int tmp = orders[i];
            orders[i] = orders[n];
            orders[n] = tmp;
        }

        if (!CheckValidate()) ShffulTile();
    }

    // 무결성 조사 <- ShffulTile
    bool CheckValidate () {
        int sum = 0;
        for (int i = 0; i < orders.Count - 1; i++) {
            if (orders[i] == -1) continue;

            for (int j = i + 1; j < orders.Count; j++) {
                if (orders[j] != -1 && orders[i] > orders[j]) sum++;
            }
        }
        return (sum % 2 == 0);
    }

    // SetTouch <- Tile
    void SetTouch (int _tileNum) {
        if (state == STATE.idle) {
            tileNum = _tileNum;
            state = STATE.touch;
        }
    }

    // Set Calc <- Tile
    void SetCalc () {
        state = STATE.calc;
    }

    // Make Tile <- Init
    void MakeTiles () {
        tiles.Clear();
        orders.Clear();

        Vector2 size = sprites[0].bounds.size;
        float w = sprites[0].rect.width;
        int n = 0;

        for (int y = 0; y < sliceCnt; y++) {
            for (int x = 0; x < sliceCnt; x++) {
                MakeSingleTile(n, size);
                orders.Add(n++);
            }
        }

        // 마지막 타일
        orders[orders.Count - 1] = -1;
        tiles[orders.Count - 1].gameObject.SetActive(false);
    }

    // Make SingleTile <- MakeTile
    void MakeSingleTile (int idx, Vector2 size) {
        GameObject tile = Instantiate(Resources.Load("Tile")) as GameObject;
        tile.transform.localScale = new Vector3(tileScale, tileScale, 1);

        // Tile에 분할한 Sprite 입히기
        SpriteRenderer render = tile.GetComponent<SpriteRenderer>();
        render.sprite = sprites[idx];
        render.material.SetInt("_count", sliceCnt);
        tile.name = "Tile" + idx;

        // Box Collider2D
        BoxCollider2D collider = tile.GetComponent<BoxCollider2D>();
        collider.size = size;
        collider.offset = new Vector2(size.x / 2, -size.y / 2);

        // tile 저장
        tiles.Add(tile.transform);
    }

    // Textrure 자르기 <- Init
    void SplitTexture () {
        // 기준 이미지
        Texture2D org = Resources.Load("Image_0", typeof(Texture2D)) as Texture2D;

        // 이미지 읽기
        Texture2D texture = Resources.Load("Image_" + imgNum, typeof(Texture2D)) as Texture2D;
        tileScale = (float)org.width / texture.width;

        // 자를 조각의 크기
        float w = texture.width / sliceCnt;
        float h = texture.height / sliceCnt;

        // Texture를 위에서부터 자르기
        sprites.Clear();
        for (int y = sliceCnt - 1; y >= 0; y--) {
            for (int x = 0; x < sliceCnt; x++) {
                Rect rect = new Rect(x * w, y * h, w, h);
                Vector2 pivot = new Vector2(0, 1);
                Sprite sprite = Sprite.Create(texture, rect, pivot);
                sprites.Add(sprite);
            }
        }
    }

    // On Button Click
    public void OnButtonClick (GameObject button) {
        switch (button.name) {
        case "BtnYes" :
            SceneManager.LoadScene("PuzzleGameTitleScene");
            break;
        case "BtnNo":
            panelQuit.SetActive(false);
            state = STATE.idle;
            startTime = Time.time - timeSpan; 
            canUI = true;
            break;
        }
    }

    // Set Origin <- Init
    void SetOrigin () {
        origin = GameObject.Find("Origin").transform;
        origin.localScale = new Vector3(tileScale * 0.6f, tileScale * 0.6f, 1);

        // Texture 읽기
        Texture2D texture = Resources.Load("Image_" + imgNum, typeof(Texture2D)) as Texture2D;
        int w = texture.width;
        int h = texture.height;

        // 새로운 Sprite 생성
        Rect rect = new Rect(0, 0, w, h);
        Sprite sprite = Sprite.Create(texture, rect, new Vector2(0, 1));

        SpriteRenderer render = origin.GetComponent<SpriteRenderer>();
        render.sprite = sprite;
        render.material.SetInt("_count", sliceCnt);
    }

    // Set Camera Orthographic Size <- Init
    void SetCamera () {
        float org = 16f / 9;
        float rate = ((float) Screen.width / Screen.height) / org;
        Camera.main.orthographicSize = (6 / rate);
    }

    // Set UI <- Init
    void SetUI () {
        panelQuit = GameObject.Find("PanelQuit");
        panelQuit.SetActive(false);

        panelComplete = GameObject.Find("PanelComplete");
        panelComplete.SetActive(false);

        txtTime = GameObject.Find("TxtTime").GetComponent<Text>();
        txtMove = GameObject.Find("TxtMove").GetComponent<Text>();
    }

    // Set Theme Music <- Init
    void SetThemeMusic () {
        if (!Settings.canMusic) return;

        AudioSource audioSrc = gameObject.AddComponent<AudioSource>();
        audioSrc.loop = true;
        audioSrc.volume = 0.3f;

        audioSrc.clip = Resources.Load("Audio/Theme") as AudioClip;
        audioSrc.Play();
    }

    // Init Game
    void InitGame () {
        sliceCnt = Settings.sliceCnt;
        imgNum = Settings.imgNum;

        SplitTexture();
        MakeTiles();

        SetOrigin();
        SetCamera();

        SetUI();
        SetThemeMusic();
    }
}
