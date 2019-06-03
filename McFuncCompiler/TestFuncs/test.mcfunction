# Test
setblock ~ ~ ~ air
say Hello world

scoreboard players set @s points 0 
scoreboard objectives add points dummy "Total Points"

give @s iron_sword`sword.json` 1

execute as @a run tellraw @s `hello`

say #ping