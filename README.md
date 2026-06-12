# Beadtz

A concert management simulation game built in Unity where you play as a music promoter. Plan concerts, manage artists, book venues, and grow your empire across multiple cities.

## Gameplay Overview

You start with a roster of unlocked artists and a limited budget. Your goal is to plan and run successful concerts to earn money and EXP, level up, and unlock new artists and cities.

### Core Loop
1. **Pick an artist** from your unlocked roster
2. **Choose a city and venue** — indoor or outdoor, each with different capacities and costs
3. **Build a setlist** from the artist's songs
4. **Buy equipment** (e.g. light sticks) to boost crowd engagement
5. **Schedule the concert** and watch it play out in real time
6. **Evaluate the concert** to collect earnings and EXP

### Key Systems

**Artists**
- Each artist has Fame and Energy stats
- Energy depletes after every concert — overworking an artist hurts performance
- Artists recover energy naturally over time or faster when placed on break
- Fame grows with successful concerts and drops when concerts are missed or flopped

**Concerts**
- Crowd energy decays in real time during a concert based on the city's music taste, the artist's condition, and your equipment
- Random events trigger mid-concert: security incidents, encores, solo performances, and more
- Overlapping concerts for the same artist are automatically marked as missed

**Cities & Venues**
- Each city has its own music taste profile — matching the artist's genre to the city boosts attendance
- Crowd size is calculated from city population, artist fame, genre fit, venue type, and a random factor
- Venues require renovation after every 3 concerts and become unavailable during that window

**Progression**
- Earn EXP and money from concert evaluations
- Level up to unlock new artists and cities
- Buy new venues to expand your reach

## Project Structure

```
Assets/
├── Scripts/
│   ├── Gameplay/       # Core game logic (Artist, Concert, Venue, Player, etc.)
│   ├── UI/             # All UI controllers and managers
│   ├── Audio/          # AudioManager
│   └── Enum/           # Game enums (Genre, VenueType, ConcertStatus, etc.)
├── Art/                # Fonts and sprites
├── Animator/           # Animation controllers
└── Tests/              # Unit tests (TestArtist)
```

## Built With

- Unity (C#)
- DOTween — animation tweening
- TextMeshPro — UI text rendering

## Submission Notes

The unit test file (`TestArtist`) is in `Assets/Tests`.
UML class diagram, sequence diagram, and unit test passing screenshot are in the `CustomProgramSubmissionRequirements` folder.
