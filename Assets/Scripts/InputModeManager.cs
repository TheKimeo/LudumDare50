using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputModeManager : Singleton<InputModeManager>
{
    public class OnModeChange : UnityEvent<InputModeManager> { }
    [SerializeField] public OnModeChange m_onModeChange = new OnModeChange();

    public Texture2D buildCursor;
    public Texture2D repairCursor;
    public Texture2D deleteCursor;

    public enum Mode
    {
        BUILD,
        REPAIR,
        DELETE,
        NONE
    }
    public Mode m_initMode = Mode.NONE;
    public KeyCode m_repairModeKey = KeyCode.Alpha2;
    public KeyCode m_deleteModeKey = KeyCode.Alpha3;



    Mode m_mode;

    public Mode GetMode()
    {
        return m_mode;
    }

    public void SetMode(Mode i_mode)
    {
        m_mode = i_mode;
        m_onModeChange?.Invoke(this);


        switch(i_mode)
        {
            case Mode.BUILD:
                Cursor.SetCursor(buildCursor, new Vector2(0.5f, 0.5f), CursorMode.Auto);
                break;
            case Mode.REPAIR:
                Cursor.SetCursor(repairCursor, new Vector2(0.5f, 0.5f), CursorMode.Auto);
                break;
            case Mode.DELETE:
                Cursor.SetCursor(deleteCursor, new Vector2(0.5f, 0.5f), CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(m_repairModeKey))
        {
            SetMode(Mode.REPAIR);
        }
        else if (Input.GetKeyDown(m_deleteModeKey))
        {
            SetMode(Mode.DELETE);
        }
    }

	public void SetMode_Repair() => SetMode( Mode.REPAIR );
	public void SetMode_Demolish() => SetMode( Mode.DELETE );
}
