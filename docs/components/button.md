---
layout: components
title: "Button"
---

Entry:

| Name | Type | Description |
|---|---|---|
| text | string | Text to draw inside the buton  |


Out:

- True, if the button have been clicked
- False, otherwise


<center>
    <div class="mermaid">
        %% Example diagram
        graph TB
            Begin[Button] --> IsVisible{Is visible ?}
            IsVisible -- No --> End
            IsVisible -- Yes --> IsHover
            subgraph Get infos
                IsHover --> IsPressed
                IsPressed --> IsReleased
            end
            IsReleased --> SetBackgroundStyle[Set background style]
            SetBackgroundStyle --> DrawBackground[Draw background]
            DrawBackground --> IsHoverForCursor{IsHover = true ?}
            IsHoverForCursor -- No --> SetTextStyle[Set text style]
            IsHoverForCursor -- Yes --> UpdateCursor[Set cursor as pointer]
            UpdateCursor --> SetTextStyle
            SetTextStyle --> DrawText[Draw text]
            DrawText --> EndElement
            EndElement --> End(Return IsReleased)

    </div>
</center>
