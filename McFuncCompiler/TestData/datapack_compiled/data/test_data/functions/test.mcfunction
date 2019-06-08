setblock ~ ~ ~ air
say `test_data:string.json`
say Hello world
say Compiler version 0.1
say Hello.
scoreboard players set @s points 0
scoreboard objectives add points dummy "Total Points"
give mitchfizz05 iron_sword`test_data:sword` 1
scoreboard players set var globals 50
scoreboard players add var globals 10
scoreboard players remove var globals 5
say 3333
scoreboard players set var globals 3333
scoreboard players reset var globals
scoreboard players set mitchfizz05 points 69
give @s iron_sword`test_data:sword.json` 1
summon minecraft:villager ~ ~ ~ `test_data:villager.json`
execute as @a run tellraw @s `test_data:hello`
execute if score tier data matches 4.. run function test_data:_funcblocks/test__0
