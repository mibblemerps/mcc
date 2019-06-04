setblock ~ ~ ~ air
say Hello world
say Compiler version 0.1
say Hello.
scoreboard players set @s points 0
scoreboard objectives add points dummy "Total Points"
scoreboard players set var globals 50
scoreboard players add var globals 10
scoreboard players remove var globals 5
scoreboard players set var globals 3333
scoreboard players reset var globals
scoreboard players set mitchfizz05 points 69
give @s iron_sword{"Damage":100} 1
execute as @a run tellraw @s ["",{"text":"[Big Brother] ","bold":true,"color":"light_purple"},"Hello ",{"selector":"@s"},"."]
function test_data:test__0
