# VisionOS Clipboard Paste Plugin

Simple, working clipboard paste functionality for VisionOS URL input.

## What This Provides

✅ **Working VisionOS clipboard access**  
✅ **Simple button-based paste functionality**  
✅ **URL validation**  
✅ **No complex dependencies**  
✅ **Minimal, focused implementation**

## Files Created

1. `VisionOSClipboard.mm` - Native VisionOS plugin (minimal)
2. `VisionOSPaste.cs` - C# clipboard interface
3. `URLInputExample.cs` - Example usage script

## Quick Setup

### 1. Add Components to Your Scene

```csharp
// Add VisionOSPaste component to a GameObject with:
- TMP_InputField (for URL input)
- Button (for paste action)
```

### 2. Configure in Inspector

- Assign `urlInputField` to your TMP_InputField
- Assign `pasteButton` to your paste Button
- Enable `enableDebugLogs` for testing

### 3. Button Setup

The paste button will automatically:
- Enable/disable based on clipboard content
- Paste clipboard text when clicked
- Handle errors gracefully

## Code Example

```csharp
public class MyURLHandler : MonoBehaviour
{
    public VisionOSPaste pasteComponent;
    public TMP_InputField urlInput;
    
    void Start()
    {
        // Configure paste component
        pasteComponent.urlInputField = urlInput;
        pasteComponent.enableDebugLogs = true;
    }
    
    // Call this from a UI button
    public void OnPasteButtonClick()
    {
        pasteComponent.PasteFromClipboard();
    }
}
```

## Integration with APIs

```csharp
public void OnURLPasted()
{
    string url = urlInput.text;
    if (!string.IsNullOrEmpty(url))
    {
        // Use with your AI3D APIs
        nanoBananaIO.SubmitRequest(url, OnImageProcessed);
        // or
        moGeIO.ProcessURL(url);
    }
}
```

## Platform Behavior

- **VisionOS/iOS**: Uses native UIPasteboard
- **Unity Editor**: Uses GUIUtility.systemCopyBuffer  
- **Other platforms**: Graceful fallback

## Why This Works

1. **Minimal scope** - Only essential clipboard functions
2. **No complex features** - Just get text and check if available
3. **Proper memory management** - Native strings handled correctly
4. **Error handling** - Won't crash if clipboard is unavailable
5. **Unity will auto-generate .meta files** - No manual meta file management

## Testing

1. Copy a URL to your system clipboard
2. Click the paste button in your app
3. URL should appear in the input field
4. Submit button should become active for valid URLs

This implementation focuses on reliability over features - it just works!
