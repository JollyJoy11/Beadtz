# Beadtz

**A real-time music management game where you build a global concert empire — one crisis at a time.**

You are a concert manager. Your job is to sign artists, book venues, and send them out on tour across the world. But when the lights go up and the crowd starts chanting, your work is just beginning.

---

## What Makes Beadtz Different

Most management games let you plan everything in peace, then watch it play out. Beadtz doesn't.

While your concerts are running live, unexpected events fire at any moment — and you have seconds to decide how to handle them. A fight breaks out in the crowd. A fan rushes the stage. Your artist is visibly falling apart on stage with 20 minutes left in the set. Do you push through or pull the plug?

And you're not just managing one concert. You can have multiple shows running simultaneously across different venues, all demanding your attention at once.

---

## Core Gameplay Loop

1. **Sign artists** — unlock solo acts and groups as you level up
2. **Plan your concert** — choose the artist, city, venue, setlist, and any equipment to bring
3. **Watch it live** — concerts run in real time while the clock ticks
4. **React to events** — timed decision prompts appear mid-show with meaningful tradeoffs
5. **Evaluate results** — crowd energy, fame, and profit determine your EXP and earnings
6. **Grow your empire** — level up to unlock new artists and new cities across the globe

---

## Live Concert Events

When a concert is ongoing, events can fire at any moment. Each one gives you a short window to decide — let it expire and you face the default consequence.

| Event | Choice A | Choice B |
|-------|----------|----------|
| **Encore** | Extend the show — crowd energy +5, artist tires faster | Wrap up — artist preserved, fame -5 |
| **Security Incident** | Call security — situation contained, +EXP | Ignore it — crowd energy -10 |
| **Crowd Losing Energy** | Deploy your best equipment to re-energise the crowd | Do nothing — crowd energy -15 |
| **Fan Rushes Stage** | Let it happen — crowd energy +15, fame +5, but artist energy -10 | Block them — no impact, stay safe |
| **Artist Exhausted** | Push through — artist energy hits 0, forced rest after show | Cut the set — concert ends early, reduced earnings |

---

## Artist Management

Artists aren't just stat blocks — they have stamina, and they burn out.

- **Solo artists** tire quickly under heavy tour schedules. The more consecutive concerts they play, the steeper the fatigue penalty.
- **Groups** distribute the load across their members, making them more resilient for long runs — but they still have a limit.

When an artist's energy drops too low, they go on mandatory rest. Book them anyway and watch the crowd notice.

---

## Cities & Venues

Each city you unlock has its own personality:

- **Population** affects how large your potential crowd is
- **Music taste** shifts dynamically — play enough of a genre in a city and the crowd warms to it over time
- **Venues** range from intimate indoor clubs to sprawling outdoor stages, each with different capacities, costs, and crowd dynamics

Venues also need maintenance. Run too many consecutive concerts at the same venue and it goes into renovation, locking you out until repairs are done.

---

## Equipment

Before each concert, you can invest in production equipment to give yourself an edge:

- **Light Show** — boosts crowd energy mid-concert
- **Fireworks** — bigger crowd energy burst
- **Light Sticks** — slows crowd energy decay throughout the whole show
- **Confetti** — a smaller, flexible crowd boost you can deploy any time

Equipment is single-use per concert, so timing matters.

---

## Progression

Your EXP and fame grow with every successful show. Level up to unlock:
- New artists (solo acts and groups)
- New cities — from Cairo to Rome to New York

Each city brings new venue options, new crowd tastes to learn, and new concert opportunities to exploit.

---

## Built With

- **Unity** (C#)
- ScriptableObject-driven data architecture
- Real-time event system with timed decision windows
- JSON save system with full session persistence

---

## Development

Beadtz started as a university project and grew into something bigger — a genre that doesn't quite exist yet. There are city builders, idle tycoons, and music rhythm games. There isn't really a game about the chaos behind a live concert at scale.

This is that game.
