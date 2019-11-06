# DESIGN 

This document summarize the source code design specifications.
> **Note:** 
> To see the diagrams written in mermaid format, install Markdown Preview Mermaid Support from extension market places.

## Scenes

Siribal application is established from following scenes

- Top
- Login
- ARGame
- Loading
- Result
- ScoreBoard

Define the behaviour of these states

```mermaid
graph TB
    style c fill:#cdf, stroke:#abf
    style d2 fill:#cdf, stroke:#abf

    a[Launch] --> b[Load cached information]
    b --> c(cache found?)
    c --yes--> d1[Load cached information]
    c --no--> e1[Login scene]
    d1 --> d2(successfully loaded?)
    d2 --yes--> f[Top scene]
    d2 --no--> e1
    e1 --Guest Login--> f
    e1 --Login by Existing User--> f
    e1 --Create User--> e1-1[Input email and password]
    e1-1 --> e1-2[Send email]
    e1-2 --> e1-3[Verify account]
    e1-3 --> e1
    f --Click Start--> g[ARGame scene]
    f --Click Score--> h[ScoreBoard]
    g --Play--> i[Playing...]
    i --TimeUp or Hit all--> j[Result scene]
    i --Force Quite--> f
    j --OK--> f
    f --Change User-->e1
```

> **NOTE**
> "Back to Top" button flow is ommitted.


