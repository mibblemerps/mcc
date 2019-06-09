# Compiled test_data:test at 2019-06-09 16:06 by McFuncCompiler
execute if score var1 globals matches 0 run function test_data:_subs/globals_0
say Hello world
say Compiler version 0.1
scoreboard players set @s points 0
scoreboard objectives add points dummy "Total Points"
scoreboard players set var globals 50
scoreboard players add var globals 10
scoreboard players remove var globals 5
say 3333  
scoreboard players set var globals
scoreboard players reset var globals
scoreboard players set mitchfizz05 points 69
execute if score tier globals < pending_tier data run say Pending tier!
give @s iron_sword{Damage:100}  1
summon minecraft:villager ~ ~ ~ {VillagerData:{profession:nitwit,level:99,type:plains},Invulnerable:1,PersistenceRequired:1,Silent:1,NoAI:1,Rotation:[110f,0f],CustomName:"\"Cashier\"",Offers:{Recipes:[{buy:{id:emerald,Count:8},sell:{id:bread,Count:6},maxUses:9999999},{buy:{id:emerald,Count:4},sell:{id:apple,Count:4},maxUses:9999999},{buy:{id:emerald,Count:12},sell:{id:egg,Count:12},maxUses:9999999},{buy:{id:emerald,Count:4},sell:{id:potato,Count:6},maxUses:9999999},{buy:{id:emerald,Count:4},sell:{id:beetroot,Count:6},maxUses:9999999},{buy:{id:emerald,Count:4},sell:{id:carrot,Count:6},maxUses:9999999},{buy:{id:emerald,Count:4},sell:{id:melon_slice,Count:6},maxUses:9999999},{buy:{id:emerald,Count:4},sell:{id:cookie,Count:12},maxUses:9999999},{buy:{id:emerald,Count:12},sell:{id:sugar,Count:16},maxUses:9999999},{buy:{id:emerald,Count:1},sell:{id:bowl,Count:1},maxUses:9999999},{buy:{id:emerald,Count:64},sell:{id:enchanted_golden_apple,Count:1},maxUses:9999999}]}}
execute as @a run tellraw @s ["",{"text":"[Big Brother] ","bold":true,"color":"light_purple"},"Hello ",{"selector":"@s"},"."]
execute unless score tier bb matches 2.. run function test_data:_subs/test_0
