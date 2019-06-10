# Test MC function file

### IF STATEMENTS ###

# Before
execute if score level data matches 10.. run say Level 10!
execute if score level data matches 10.. run scoreboard players set level data 0

# After
# "if" <if condition for /execute> "{"
if score level data matches 10.. {
	# Level 10!
	say Level 10!
	scoreboard players set level data 0
}


### VARIABLES ###

# Before
scoreboard players set points globals 5
scoreboard players operation points globals = level globals
scoreboard players operation points globals += level globals

# After
$points = 5
$points = level globals
$points += level globals


# Before
scoreboard players add points globals 1
scoreboard players remove points globals 1

# After
$points += 1
$points -= 1


### IMPORT JSON ###
give @s iron_sword`sword_nbt.json` 1


### DELAY ###

# Before
# - Typically summon a command block minecart that falls or has a PortalCooldown tag.

# After
delay 200
# (abstracts away the hackiness)
