using UnityEngine;
using System.Collections;

public class ChessPanel : GamePanel
{

    public ChessBoardgame chessBoardGame;
    public GameObject chessObject;
    public GamePanel mainMenu;
    public override IEnumerator Enter()
    {
        if (moving)
            yield break;

        if (chessObject)
            chessObject.SetActive(false);
        moving = true;

        foreach (GameObject obj in panelObjects)
        {
            obj.gameObject.SetActive(false);
        }

        Vector2 start = DefaultStartPosition(1);
        Vector2 end = screenCenter;
        transform.localPosition = start;


        yield return null;
        transform.MoveToLocal(end, animTime);
        yield return new WaitForSeconds(animTime / 2f);
        if (chessObject)
        {
            chessObject.transform.position = new Vector3(end.x + MathOperations.ScreenWidth, end.y, chessObject.transform.position.y);
            chessObject.SetActive(true);
            chessBoardGame.PrepareGame();
            chessObject.transform.MoveTo(end, animTime);
            yield return new WaitForSeconds(animTime / 2f);
        }
        yield return new WaitForSeconds(animTime / 2f);

        moving = false;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        chessBoardGame.OnEnd += OnGameEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        chessBoardGame.OnEnd -= OnGameEnd;
    }

    public override IEnumerator Exit()
    {
        if (moving)
            yield break;

        moving = true;

        Vector2 start = screenCenter;
        Vector2 end = DefaultStartPosition(-1);
        transform.localPosition = start;
        if (chessObject)
        {
            chessObject.transform.position = new Vector3(start.x, start.y, chessObject.transform.position.z);
            chessObject.SetActive(true);
            chessObject.transform.MoveTo(new Vector3(start.x - MathOperations.ScreenWidth, start.y, chessObject.transform.position.z), animTime);
            yield return new WaitForSeconds(animTime / 2f);
        }

        transform.MoveToLocal(end, animTime);
        yield return new WaitForSeconds(animTime);

        if (chessObject)
            chessObject.SetActive(false);
        gameObject.SetActive(false);
        moving = false;
    }

    public void OnGameEnd()
    {
        SceneController.instance.ChangePanel(mainMenu);
    }

    public override void OnBack()
    {

    }
}
