# RePrint Combat Guide - Part 1: Combat Basics

## Overview

RePrint is a turn-based roguelike combat game where you build Chain with **Starter** actions and spend it with **Finisher** actions. Each turn, you have **3 AP** to spend on actions by default, and enemies telegraph their attacks via **Intents**.

---

## Action Points (AP)

- **Starting AP**: 3 per turn
- **Regeneration**: Full AP restored at turn start
- **Spending**: Most actions cost 1-2 AP

| Action Type | Typical AP Cost |
|-------------|-----------------|
| Starter     | 1 AP            |
| Finisher    | 2 AP            |
| Utility     | 1 AP            |
| Charge-based| 0 AP (uses charges instead of AP)|

---

## Chain System

**Chain** is the core combat resource that connects Starters and Finishers.

### Building Chain
- **Starters** grant Chain on hit (e.g., Quick Slash: +1 Chain, some Starters don't grant chain by default like Heavy Slash)
- **Multi-hit actions** grant Chain per hit (e.g., 2x Slash: +1 Chain × 2 hits)

### Spending Chain
- **Finishers** multiply their damage by current Chain
- Example: Rift Cut at 5 Chain = 5 × 4 = 20 damage
- Some Finishers are multi-hit and deal one hit per Chain 
- Example: Flurry: Deals 3-5 dmg per chain. The amount of hits in this instance is equal to chain spent. 
- Chain is consumed when Finisher resolves

### Chain Reset
Your Chain resets to 0 when you take **physical damage** from enemies.

**Does NOT reset Chain**:
- Bleed damage
- Shock damage
- Burn damage
- Bio damage
- Any status effect damage

### Chain Strategy
- Build Chain early with Starters
- Use Dodge/Shield to protect your Chain
- Cash out with Finisher before taking hits
- Status damage is safe - doesn't break Chain

---

## Action Types

### Starters (Build Chain)

**Purpose**: Deal damage and build Chain for Finishers.

**Core Starters** (No cooldown penalties):
- **Quick Slash**: 6 dmg, +1 Chain — Reliable, consistent starter
- **Variable Blade**: 1-2 hits × 3 dmg, +1 Chain/hit — Higher variance, higher potential
- **2x Slash**: 2 hits × 2 dmg, +1 Chain/hit — Multi-hit, great for mods

**Advanced Starters** (Gain cooldown per manual Overclock; typically 0 cooldown at base +1 cooldown per manual OC):
- **Heavy Slash**: 8-12 dmg, no Chain — Pure damage, no chain building
- **Hyper Slash**: 4 dmg, +2-4 Chain — Low damage, high chain gain
- **Arc Blade**: 6 dmg AoE, +1 Chain/enemy — Hits all enemies
- **Chaotic Blade**: 1-2 hits × 5 dmg, random targets — Unpredictable multi-target

### Finishers (Spend Chain)

**Purpose**: Convert accumulated Chain into massive damage.

**Core Finishers**:
- **Rift Cut**: 4 dmg per Chain — Simple, reliable. At 5 Chain = 20 damage

**Advanced Finishers**:
- **Flurry**: 3-5 dmg per Chain (Multi-Hit) — Variable damage, single target
- **Slice n' Dice**: 2-6 dmg per Chain (AoE Multi-Hit) — Spreads hits randomly across all enemies

**Special Finishers**:
- **Culminate**: 10-12 dmg per Starter used this combat — Doesn't consume Chain, resets starter count

### Utility Actions

**Purpose**: Defensive options and buffs.

**Core Utility**:
- **Dodge**: Gain 3-5 Dodge — Reduces incoming damage
- **ShieldUp**: +3 Shield — Persistent damage absorption

**Special Utility**:
- **Flicker**: Gain 5-7 Dodge (Charge-based) — No cooldown, uses charges
- **Shadow Image: Decoy**: Summon a decoy that intercepts one enemy hit
- **Shadow Image: Emulate**: Creates a shadow that repeats your actions at 25% damage

### Charge Actions

Some actions use **Charges** instead of AP:
- **Cost**: 0 AP, 1 Charge per use
- **Restoration**: Charges restore between battles
- **Examples**: Kunai (10 dmg starter), Flicker (dodge), Catalyst (+3-5 Chain)

Charge actions can be used alongside AP actions in the same turn.

---

## Damage Types & Status Effects

### Physical Damage
Standard damage type. Resets your Chain when received from enemies.

### Bleed (Threshold: 10)
- **Application**: Stacks on target
- **Trigger**: At 10 stacks → **Hemorrhage**
- **Hemorrhage Effect**: 33% max HP damage (25% for elites, 18% for bosses)
- **Post-Trigger**: Bleed persists
- **Note**: Bleed damage does NOT reset player Chain

### Shock (Threshold: 10)
- **Application**: Stacks on target
- **Trigger**: At 10 stacks → **Stasis**
- **Stasis Effect**: Target skips their next turn, timers frozen
- **Post-Trigger**: Shock persists
- **Note**: Shock damage does NOT reset player Chain

### Burn (Threshold: 10)
- **Application**: Stacks on target
- **Trigger**: At 10 stacks → **Overheat**
- **Overheat Effect**: 1-3 damage per turn for 3 turns per Overheat stack
- **Post-Trigger**: Burn persists

### Bio (Threshold: 10)
- **Application**: Stacks on target
- **Trigger**: At 10 stacks → **System Collapse**
- **System Collapse Effect**: 3-turn countdown to death
- **Post-Trigger**: All Bio damage is removed and player cannot recieve any additional Bio damage. 

### Breach (Threshold: 10)
- **Trigger**: At 10 stacks → **Breached**
- **Breached Effect**: Player receives a random Hack effect. 

### Chain Damage
- Special enemy attack type that steals Chain from the player
- Can be reduced by Dodge

---

## Enemy Intent System

Enemies telegraph their next action via **Intents** displayed above their unit.

### Intent Icon Examples

| Icon | Intent Type | Description |
|------|-------------|-------------|
| Sword | Damage | Direct physical damage |
| Water Drop | Bleed | Applies Bleed stacks |
| Bolt | Shock | Applies Shock stacks |
| Fire | Burn | Applies Burn stacks |
| Skull | Bio | Applies Bio stacks |
| Sprint | Defensive | Dodging or defensive action |
| Buff | Buff | Applying buffs (Sharpen, etc.) |
| Link | Chain Damage | Stealing player Chain |

### Reading Intents

- **Damage Range**: Shows as "5-7" meaning 5 to 7 randomly rolled damage 
- **Status Amount**: Shows stacks to be applied

Use intents to prioritize targets and plan defensive actions.

---

## Overclocking

**Overclock (OC)** enhances actions to higher rarity tiers.

### Overclock Tiers if starting from Base level (0-4)

| OC Level | Rarity |
|----------|--------|
| OC 0 | Common |
| OC 1 | Uncommon |
| OC 2 | Rare |
| OC 3 | Ethereal |
| OC 4 | Mythic |

### Example: Quick Slash Progression if starting at Base level. s

| OC | Damage | Chain |
|----|--------|-------|
| 0  | 6      | +1    |
| 1  | 8      | +2    |
| 2  | 12     | +3    |
| 3  | 16     | +4    |
| 4  | 25     | +6    |

### Cooldown Scaling

**Advanced** actions gain cooldown at higher OC levels:
- **Advanced Starters**: +1 cooldown per OC level
- **Core Finishers**: Base 1 + 1 per OC level
- **Advanced Finishers**: Base 2 + 1 per OC level

This creates a trade-off: higher OC means more power but longer cooldowns.

---

## Dodge System

Dodge reduces incoming damage from enemy attacks.

### Gaining Dodge

- **Dodge Action**: Gain 3-5 Dodge (OC increases range)
- **Flicker**: Gain 5-7 Dodge (charge-based)
- **Deflect**: Gain 3 Dodge + reflect damage back
- **Mods**: Various mods grant passive or triggered Dodge

### Dodge Assignment

When you gain Dodge, you can assign it in two ways:

**1. Targeted Dodge**
- Click an enemy to assign Dodge specifically to them
- Any remaining Dodge is used on secondary enemy attacks. 

**2. General Dodge**
- Click the player icon (or use without targeting)
- Dodge goes to a general pool
- Blocks damage with unspecified priority

### Damage Resolution Order

1. **Assigned Dodge First**: If enemy has Dodge assigned to them, use it first
2. **Leftover Pool**: Any remaining damage uses the general Dodge pool
3. **Shield**: After Dodge is exhausted, Shield absorbs damage
4. **HP**: Finally, damage hits player HP

### Dodge Redistribution

When an enemy dies with Dodge assigned to them:
- That Dodge returns to the **general pool**
- Can then block damage from other enemies

### Example

You have 10 total Dodge:
- 6 assigned to Stalker
- 4 in general pool

**Stalker attacks for 8 damage**:
- Uses 6 assigned Dodge → 2 damage remaining
- Uses 2 from general pool → 0 damage taken
- General pool now has 2 remaining

**Vagabond attacks for 5 damage**:
- No assigned Dodge for Vagabond
- Uses 2 from general pool → 3 damage taken

---

## Shield System

Shield provides persistent damage absorption that doesn't expire at turn end.


### Shield Properties
- **Persistent**: Unlike Dodge, Shield carries between turns
- **Stacks**: Multiple Shield sources add together
- **No Cap**: No maximum Shield limit

### Damage Absorption Order

When taking damage:
1. **Dodge** absorbs first (if assigned to attacker or in general pool)
2. **Shield** absorbs second (after Dodge depleted)
3. **HP** takes remaining damage

---

## Total Damage Preview

When enabled, a **damage preview** appears below each enemy showing:

### What It Shows
- Total queued damage from all actions targeting that enemy
- Breakdown by damage type (Physical, Shock, Bleed, Burn)
- Accounts for Overclock bonuses and mod multipliers

### Preview Format
```
[Sword] 15-22    ← Physical damage range
[Bolt] 4-7       ← Shock damage range
```

### Hover Tooltip

Hovering the preview shows detailed breakdown:
- Each action's contribution
- Base damage calculation
- Active multipliers (Combo mods, Gambit, etc.)
- Final damage after multipliers

### Using the Preview

- **Target Selection**: See which enemy takes lethal damage
- **Overkill Check**: Avoid wasting damage on low-HP enemies
- **Status Threshold**: Track status stacks approaching trigger threshold
- **Mod Optimization**: Verify multiplier mods are activating correctly

---

## Combat Flow Summary

1. **Turn Start**: Receive full AP, view enemy intents
2. **Queue Actions**: Select Starters, Finishers, Utility actions
3. **Assign Targets**: Click enemies to target attacks/dodge
4. **Commit Turn**: Actions resolve in queue order
5. **Enemy Turn**: Enemies execute their telegraphed intents.
6. **Enemy Turn Order**: Enemies that have Dodge assigned to them always act first. If no Dodge is assigned, they act from left to right, Unless a Boss is present. Boss's will always act first if no dodge is assigned, then enemies act left to right(wip, may change later).  
7. **Repeat**: Continue until all enemies defeated

### Recommended Turn Structure

1. Use **Starters** to build Chain
2. Use **Utility** to defend against threatening intents
3. Use **Finisher** to cash out Chain damage
4. Check damage preview to ensure kills

---

## Quick Reference

| Mechanic | Key Point |
|----------|-----------|
| AP | 3 per turn, restored at turn start |
| Chain | Built by Starters, spent by Finishers, resets on direct damage |
| Shield | Persistent defense, absorbs after Dodge |
| Overclock | Enhances actions (0-4), may add cooldown |
| Status Threshold | 10 for normal, 12 elite, 15 boss |
| Dodge | Assigned per-enemy or general pool, turn-based |
| Intents | Telegraph enemy actions - plan accordingly |
| Damage Order | Dodge → Shield → HP |
