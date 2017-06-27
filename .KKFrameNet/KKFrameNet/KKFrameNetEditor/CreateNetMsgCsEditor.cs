using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
/// <summary>
/// 消息创建编辑器
/// 用于创建保存网络消息的cs文件
/// Author:StraussDu
/// </summary>
public class NetField 
{
    public string type;
    public string name;
    public string note;
    public int length;
    public bool _bCanSkip;
}
public class CreateNetMsgCsEditor : EditorWindow {
    private string[] _strArTypeNameReq = { "byte","byte[]", "short","ushort", "short[]","int", "int[]", "string", "bool" };
    private string[] _strArTypeNameResp = { "byte", "byte[]", "short", "ushort","int", "int[]", "float","double","long","double","string", "bool" };
    private int _nCreateTypeIndex;
    private int lastIndex;
    private string[] createTypeArray = { "Req File", "Resp File" };
    private int _nMainID;
    private int _nSubID;
    private int totalLength;
    private string _strNameSpace = "";
    private int _nLength = -1;
    private string fieldName = "";
    private string _strNote = "";
    private string fileName = "";
    public List<NetField> _strlFieldWrite = new List<NetField>();
    private int _nTypeIndex = 0;
    private static CreateNetMsgCsEditor _instance;
    [MenuItem("Tools/Net/CreateMsgCSFile")]
    static void CreateMyWindow() 
    {
        _instance = EditorWindow.GetWindow<CreateNetMsgCsEditor>();
        _instance.Show();
    }
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        _nCreateTypeIndex = EditorGUILayout.Popup("Choose The Type", _nCreateTypeIndex, createTypeArray);
        if (lastIndex != _nCreateTypeIndex)
        {
            _nTypeIndex = 0;
        }
        lastIndex = _nCreateTypeIndex;
        _strNameSpace = EditorGUILayout.TextField("Please Input NameSpaceName", _strNameSpace);
        fileName = EditorGUILayout.TextField("Please Input FileName",fileName);
        _nMainID = EditorGUILayout.IntField("MainID", _nMainID);
        _nSubID = EditorGUILayout.IntField("SubID", _nSubID);
        if (_strlFieldWrite.Count != 0) 
        {
            EditorGUILayout.BeginVertical();
            for (int i = 0; i < _strlFieldWrite.Count; i++) 
            {
                EditorGUILayout.BeginHorizontal();
                string s = "";
                if (_strlFieldWrite[i].type == "string" || _strlFieldWrite[i].type .Contains("[]"))
                {
                    s = _strlFieldWrite[i].type + " " + _strlFieldWrite[i].name +" //"+_strlFieldWrite[i].note+ " Length:" + _strlFieldWrite[i].length;
                }
                else 
                {
                    s = _strlFieldWrite[i].type + " " + _strlFieldWrite[i].name+" //"+_strlFieldWrite[i].note;
                }
                EditorGUILayout.LabelField(s, GUILayout.Width(300));
                if (GUILayout.Button("Remove This Field"))
                {
                    _strlFieldWrite.Remove(_strlFieldWrite[i]);
                    StringClear();
                }
                EditorGUILayout.EndHorizontal();
            }
             EditorGUILayout.EndVertical();
        }
        EditorGUILayout.BeginHorizontal();
        if (_nCreateTypeIndex == 0)
        {
            _nTypeIndex = EditorGUILayout.Popup(_nTypeIndex, _strArTypeNameReq, GUILayout.Width(50));
            fieldName = EditorGUILayout.TextField(fieldName, GUILayout.Width(100));
            _strNote = EditorGUILayout.TextField("Note", _strNote);

            if (_strArTypeNameReq[_nTypeIndex] == "string" || _strArTypeNameReq[_nTypeIndex].Contains("[]"))
            {
                _nLength = EditorGUILayout.IntField("Length", _nLength);
            }
        }
        else 
        {
            _nTypeIndex = EditorGUILayout.Popup(_nTypeIndex, _strArTypeNameResp, GUILayout.Width(50));
            fieldName = EditorGUILayout.TextField(fieldName, GUILayout.Width(100));
            _strNote = EditorGUILayout.TextField("Note", _strNote);

            if (_strArTypeNameResp[_nTypeIndex] == "string" || _strArTypeNameResp[_nTypeIndex].Contains("[]"))
            {
                _nLength = EditorGUILayout.IntField("Length", _nLength);
            }
        }     

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Add New Field")) 
        {
            NetField nf = new NetField();
            if (_nCreateTypeIndex == 0)
            {
                nf.type = _strArTypeNameReq[_nTypeIndex];
            }
            else 
            {
                nf.type = _strArTypeNameResp[_nTypeIndex];
            }
            nf.name = fieldName;
            nf.note = _strNote;
            if (nf.type == "string" || nf.type .Contains("[]"))
            {
                nf.length = _nLength;
            }
            else 
            {
                nf.length = -1;
                switch (nf.type) 
                {
                    case "short":
                        nf.length = 2;
                        break;
                    case "ushort":
                        nf.length = 2;
                        break;
                    case "double":
                        nf.length = 8;
                        break;
                    case "byte":
                        nf.length = 1;
                        break;
                    case "bool":
                        nf.length = 1;
                        break;
                }
                if (nf.length == -1) 
                {
                    nf.length = 4;
                }
            }
            nf._bCanSkip = false;
            _strlFieldWrite.Add(nf);
            StringClear();
        }
        if (_strlFieldWrite.Count > 0) 
        {
            if (GUILayout.Button("Create New CS File"))
            {
                if (_nCreateTypeIndex == 0)
                {
                    CreateReqCsFile();
                    _nMainID = 0;
                    _nSubID = 0;
                    totalLength = 0;
                }
                else 
                {
                    CreateRespCsFile();
                    _nMainID = 0;
                    _nSubID = 0;
                    totalLength = 0;
                }
                AssetDatabase.Refresh();
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();
    }
    void StringClear() 
    {
        fieldName = "";
        _nLength = -1;
        _strNote = "";
    }
    private void CreateRespCsFile() 
    {
        string path = EditorUtility.SaveFilePanel("Save The Msg CS", "", fileName + ".cs", "cs");

        StreamWriter sw = new StreamWriter(path, false);
        Write(sw, 0, "using UnityEngine;");
        Write(sw, 0, "using System.Collections;");
        Write(sw, 0, "using KK.Frame.Net;");
        Write(sw, 0, "");
        Write(sw, 0, "namespace " + _strNameSpace);
        Write(sw, 0, "{");
        Write(sw, 1, "[ProtoNetMsg(mainID = " + _nMainID + ", " + "subID = " + _nSubID + ", " + "msgTyp = ProtoNetMsgAttribute.MsgType.RespNtf)]");
        Write(sw, 1, string.Format("public class {0} : CMD_Base_RespNtf", fileName));
        Write(sw, 1, "{");
        for (int i = 0; i < _strlFieldWrite.Count; i++)
        {
            string s = "public " + _strlFieldWrite[i].type + " " + _strlFieldWrite[i].name + ";  //" + _strlFieldWrite[i].note;
            Write(sw, 2, s);
        }
        Write(sw, 0, "");
        Write(sw, 2, "public override CMD_Base_RespNtf Deserialize(ByteBuffer buf)");
        Write(sw, 2, "{");
        int totalAlignment = 0;
        int differ = 0;
        int writeIndex = 999;
        bool _bIsAddToEnd = false;
        for (int i = 0; i < _strlFieldWrite.Count; i++)
        {
            NetField nf = _strlFieldWrite[i];
            string s = nf.type;
            if (s == "string" || s.Contains("[]"))
            {
                string temp = s.Substring(0, 1).ToUpper();
                temp += s.Substring(1, s.Length - 1);
                string f = "";
                if (!s.Contains("[]"))
                {
                    f = nf.name + "=" + "buf.Read" + temp + "(" + nf.length + ");";
                }
                else 
                {
                    if (s == "byte[]")
                    {
                        string ft = temp.Replace("[]", "s");
                        f = nf.name + "=" + "buf.Read" + ft + "(" + nf.length + ");";
                    }
                    else
                    {
                        string ft = temp.Replace("[]", "Array");
                        f =nf.name + "=" + "buf.Read" + ft + "(" + nf.length + ");";;
                    }
                }
                if (i == writeIndex)
                {
                    AutoReadByte(sw, differ);
                    totalAlignment += differ;
                }
                Write(sw, 3, f);
            }
            else
            {
                string temp = s.Substring(0, 1).ToUpper();
                if (s == "ushort")
                {
                    string temptwo = s.Substring(1, 1).ToUpper();
                    temp += temptwo;
                    temp += s.Substring(2, s.Length - 2);
                }
                else
                {
                    temp += s.Substring(1, s.Length - 1);
                }
                string f = nf.name + " = " + "buf.Read" + temp + "();";
                if (i == writeIndex)
                {
                    AutoReadByte(sw, differ);
                    totalAlignment += differ;
                }
                Write(sw, 3, f);
            }
            if (nf._bCanSkip == false)
            {
                if (nf.length % 4 != 0)
                {
                    int[] byteMessageArray = BytesAlignment(nf, i);
                    writeIndex = byteMessageArray[0];
                    if (writeIndex >= _strlFieldWrite.Count)
                    {
                        _bIsAddToEnd = true;
                    }
                    differ = byteMessageArray[1];
                }
            }
        }
        if (_bIsAddToEnd)
        {
            AutoReadByte(sw, differ);
            totalAlignment += differ;
        }
        Write(sw, 3, "return this;");
        Write(sw, 2, "}");
        Write(sw, 2, "public override void Process()");
        Write(sw, 2, "{");
        Write(sw, 0, "");
        Write(sw, 2, "}");
        Write(sw, 2, "public static CMD_Base_RespNtf CreateInstance()");
        Write(sw, 2, "{");
        Write(sw, 3, "return new "+fileName+"();");
        Write(sw, 2, "}");
        Write(sw, 1, "}");
        Write(sw, 0, "}");
        sw.Close();
        _strlFieldWrite.Clear();
        fileName = "";
        fieldName = "";
        _strNameSpace = "";
    }
    private void CreateReqCsFile() 
    {
        
        string path = EditorUtility.SaveFilePanel("Save The Msg CS","",fileName+".cs","cs");
        StreamWriter sw = new StreamWriter(path,false);
        Write(sw, 0, "using UnityEngine;");
        Write(sw, 0, "using System.Collections;");
        Write(sw, 0, "using KK.Frame.Net;");
        Write(sw, 0, "");
        Write(sw, 0, "namespace "+_strNameSpace);
        Write(sw, 0, "{");
        Write(sw, 1, "[ProtoNetMsg(mainID = "+_nMainID+", "+"subID = "+_nSubID+", "+"msgTyp = ProtoNetMsgAttribute.MsgType.Req)]");
        Write(sw, 1, string.Format("public class {0} : CMD_Base_Req", fileName));
        Write(sw, 1, "{");
        Write(sw, 2, "public override short MainId { get { return " + _nMainID + "; }}");
        Write(sw, 2, "public override short SubId { get { return " + _nSubID + "; }}");
        Write(sw, 0, "");
        Write(sw, 2, "// 消息字段///");
        Write(sw, 0, "");
        for (int i = 0; i < _strlFieldWrite.Count; i++)
        {
            string s = "public " + _strlFieldWrite[i].type + " " + _strlFieldWrite[i].name + ";  //" + _strlFieldWrite[i].note;
            Write(sw, 2, s);
        }
        Write(sw, 0, "");
        Write(sw, 2, "protected override void WriteData(ByteBuffer buffer)");
        Write(sw, 2, "{");
        int totalAlignment = 0;
        int differ = 0;
        int writeIndex = 999;
        bool _bIsAddToEnd = false;
        for (int i = 0; i < _strlFieldWrite.Count; i++)
        {   
            NetField nf = _strlFieldWrite[i];
            string s = nf.type;                 
            if (s == "string" || s.Contains("[]"))
            {
                string temp = s.Substring(0, 1).ToUpper();
                temp += s.Substring(1, s.Length-1);
                string f = "";
                if (!s.Contains("[]")) 
                {
                    f = "buffer.Write" + temp + "(" + "KKNetUtil.Get4scalPasswordCN(" + nf.name + "," + nf.length + "));";
                }
                else
                {
                    if (s == "byte[]")
                    {
                        string ft = temp.Replace("[]", "s");
                        f = "buffer.Write" + ft + "(" + nf.name + ");";
                    }
                    else 
                    {
                        string ft  = temp.Replace("[]", "Array");
                        f = "buffer.Write" + ft + "(" + nf.name + ");";
                    } 
                }
                if (i == writeIndex) 
                {
                    AutoWriteByte(sw, differ);
                    totalAlignment += differ;
                }
                Write(sw, 3, f);
            }
            else 
            {
                string temp = s.Substring(0, 1).ToUpper();
                if (s == "ushort")
                {
                    string temptwo = s.Substring(1, 1).ToUpper();
                    temp += temptwo;
                    temp += s.Substring(2, s.Length - 2);
                }
                else 
                {
                    temp += s.Substring(1, s.Length - 1);
                }
                string f =  "buffer.Write" + temp + "(" + nf.name + ");";
                if (i == writeIndex)
                {
                    AutoWriteByte(sw, differ);
                    totalAlignment += differ;
                }
                Write(sw, 3, f);
            }
            if (nf._bCanSkip == false)
            {
                if (nf.length % 4 != 0) 
                {
                    int[] byteMessageArray = BytesAlignment(nf, i);
                    writeIndex = byteMessageArray[0];
                    if (writeIndex >= _strlFieldWrite.Count) 
                    {
                        _bIsAddToEnd = true;
                    }
                    differ = byteMessageArray[1];
                }
            }
        }
        if (_bIsAddToEnd) 
        {
            AutoWriteByte(sw, differ);
            totalAlignment += differ;
        }
        Write(sw, 2, "}");
        Write(sw, 0, "");
        Write(sw, 2, "protected override int GetDataSize()");
        Write(sw, 2, "{");
        for (int i = 0; i < _strlFieldWrite.Count; i++)
        {
            totalLength += _strlFieldWrite[i].length;           
        }
        totalLength += totalAlignment;
        Write(sw, 3, "return NetDefine.HEAD_LEN + " + totalLength+";");
        Write(sw, 2, "}");
        Write(sw, 2, "public static CMD_Base_Req CreateInstance()");
        Write(sw, 2, "{");
        Write(sw, 3, "return new "+fileName+"();");
        Write(sw, 2, "}");
        Write(sw, 1, "}");
        Write(sw, 0, "}");
        sw.Close();
        _strlFieldWrite.Clear();
        totalAlignment = 0;
        fileName = "";
        fieldName = "";
        _strNameSpace = "";
    }
    void AutoReadByte(StreamWriter sw, int differ) 
    {
        if (differ == 1)
        {
            Write(sw, 3, "buf.ReadByte();");
        }
        else
        {
            Write(sw, 3, "buf.ReadBytes(" + differ + ");");
        }
    }
    void AutoWriteByte(StreamWriter sw,int differ) 
    {
        if (differ == 1)
        {
            Write(sw, 3, "buffer.WriteByte(0);");
        }
        else
        {
            Write(sw, 3, "buffer.WriteBytes(new byte[" + differ + "]);");
        }
    }
    int[] BytesAlignment(NetField nf,int index) 
    {
        int[] message = new int[2];
        int _nComboByteLength = 0;
        int byteFieldConunt = index + 1;
        if (nf.length % 4 != 0)
        {
            for (int b = index + 1; b < _strlFieldWrite.Count; b++)
            {
                if (_strlFieldWrite[b].type.ToUpper().Contains("BYTE"))
                {
                    _nComboByteLength += _strlFieldWrite[b].length;
                    byteFieldConunt = b + 1;
                    _strlFieldWrite[b]._bCanSkip = true;
                }
                else
                {
                    break;
                }
            }
            int temp = nf.length + _nComboByteLength;
            message[0] = byteFieldConunt;
            message[1] = 4 - (temp % 4);
        }
        return message;
    }
    void Write(StreamWriter sw, int nTableCount, string strCon, bool bAddNewLine = true)
    {
        string strContent = "";
        for (int i = 0; i < nTableCount; ++i)
        {
            strContent += "\t";
        }
        strContent += strCon;
        if (bAddNewLine)
        {
            strContent += "\n";
        }
        sw.Write(strContent);
    }
}
