#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

extern "C" {
    
    /// Get clipboard text - minimal implementation
    char* _getClipboardText() {
        @autoreleasepool {
            UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
            NSString *text = pasteboard.string;
            
            if (text && text.length > 0) {
                const char* cString = [text UTF8String];
                if (cString) {
                    char* result = (char*)malloc(strlen(cString) + 1);
                    strcpy(result, cString);
                    return result;
                }
            }
            
            return NULL;
        }
    }
    
    /// Check if clipboard has text
    bool _hasClipboardText() {
        @autoreleasepool {
            UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
            NSString *text = pasteboard.string;
            return (text != nil && text.length > 0);
        }
    }
    
}
