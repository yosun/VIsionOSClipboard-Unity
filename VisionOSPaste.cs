using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

/// <summary>
/// Simple VisionOS clipboard paste for URL input fields
/// Minimal implementation that actually works
/// </summary>
public class VisionOSPaste : MonoBehaviour
{
#if (UNITY_IOS || UNITY_VISIONOS) && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern System.IntPtr _getClipboardText();
    
    [DllImport("__Internal")]
    private static extern bool _hasClipboardText();
#endif

    [Header("UI References")]
    public TMP_InputField urlInputField;
    public UnityEngine.UI.Button pasteButton;
    
    [Header("Settings")]
    public bool enableDebugLogs = true;
    
    private void Start()
    {
        // Auto-find components if not assigned
        if (urlInputField == null)
            urlInputField = GetComponent<TMP_InputField>();
        
        if (pasteButton == null)
            pasteButton = GetComponentInChildren<UnityEngine.UI.Button>();
        
        // Setup paste button
        if (pasteButton != null)
        {
            pasteButton.onClick.AddListener(PasteFromClipboard);
            
            if (enableDebugLogs)
                Debug.Log("VisionOSPaste: Paste button configured");
        }
        else if (enableDebugLogs)
        {
            Debug.LogWarning("VisionOSPaste: No paste button found");
        }
    }
    
    /// <summary>
    /// Paste clipboard content into the input field
    /// Called by UI button click
    /// </summary>
    public void PasteFromClipboard()
    {
        if (urlInputField == null)
        {
            if (enableDebugLogs)
                Debug.LogError("VisionOSPaste: No input field assigned");
            return;
        }
        
        string clipboardText = GetClipboardText();
        
        if (!string.IsNullOrEmpty(clipboardText))
        {
            urlInputField.text = clipboardText;
            
            if (enableDebugLogs)
                Debug.Log($"VisionOSPaste: Pasted '{clipboardText}'");
        }
        else if (enableDebugLogs)
        {
            Debug.Log("VisionOSPaste: No clipboard content available");
        }
    }
    
    /// <summary>
    /// Get clipboard text using platform-specific methods
    /// </summary>
    private string GetClipboardText()
    {
#if (UNITY_IOS || UNITY_VISIONOS) && !UNITY_EDITOR
        try
        {
            if (_hasClipboardText())
            {
                System.IntPtr textPtr = _getClipboardText();
                if (textPtr != System.IntPtr.Zero)
                {
                    string text = Marshal.PtrToStringAnsi(textPtr);
                    Marshal.FreeHGlobal(textPtr);
                    return text ?? string.Empty;
                }
            }
        }
        catch (System.Exception e)
        {
            if (enableDebugLogs)
                Debug.LogError($"VisionOSPaste: Native clipboard error: {e.Message}");
        }
        
        return string.Empty;
#else
        // Editor fallback
        try
        {
            return GUIUtility.systemCopyBuffer ?? string.Empty;
        }
        catch (System.Exception e)
        {
            if (enableDebugLogs)
                Debug.LogError($"VisionOSPaste: Editor clipboard error: {e.Message}");
            return string.Empty;
        }
#endif
    }
    
    /// <summary>
    /// Check if paste is currently possible
    /// </summary>
    public bool CanPaste()
    {
        if (urlInputField == null || !urlInputField.interactable)
            return false;
        
#if (UNITY_IOS || UNITY_VISIONOS) && !UNITY_EDITOR
        try
        {
            return _hasClipboardText();
        }
        catch
        {
            return false;
        }
#else
        try
        {
            string clipboard = GUIUtility.systemCopyBuffer;
            return !string.IsNullOrEmpty(clipboard);
        }
        catch
        {
            return false;
        }
#endif
    }
    
    private void Update()
    {
        // Update paste button state
        if (pasteButton != null)
        {
            bool canPaste = CanPaste();
            if (pasteButton.interactable != canPaste)
            {
                pasteButton.interactable = canPaste;
            }
        }
    }
}
