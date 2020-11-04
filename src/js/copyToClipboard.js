// Code taken from
// https://stackoverflow.com/questions/400212/how-do-i-copy-to-the-clipboard-in-javascript


const copyToClipboard = (function initClipboardText() {
    const textarea = document.createElement('textarea');

    // Move it off-screen.
    textarea.style.cssText = 'position: absolute; left: -99999em';

    // Set to readonly to prevent mobile devices opening a keyboard when
    // text is .select()'ed.
    textarea.setAttribute('readonly', true);

    document.body.appendChild(textarea);

    return function setClipboardText(text) {
        textarea.value = text;

        // Check if there is any content selected previously.
        const selected = document.getSelection().rangeCount > 0 ?
            document.getSelection().getRangeAt(0) : false;

        // iOS Safari blocks programmatic execCommand copying normally, without this hack.
        // https://stackoverflow.com/questions/34045777/copy-to-clipboard-using-javascript-in-ios
        if (navigator.userAgent.match(/ipad|ipod|iphone/i)) {
            const editable = textarea.contentEditable;
            textarea.contentEditable = true;
            const range = document.createRange();
            range.selectNodeContents(textarea);
            const sel = window.getSelection();
            sel.removeAllRanges();
            sel.addRange(range);
            textarea.setSelectionRange(0, 999999);
            textarea.contentEditable = editable;
        }
        else {
            textarea.select();
        }

        try {
            const result = document.execCommand('copy');

            // Restore previous selection.
            if (selected) {
                document.getSelection().removeAllRanges();
                document.getSelection().addRange(selected);
            }

            return result;
        }
        catch (err) {
            console.error(err);
            return false;
        }
    };
})();

export default copyToClipboard;
