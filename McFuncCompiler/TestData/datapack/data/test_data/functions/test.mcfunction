# Test
define compiled_header File {file}

import test_data:globals

say `test_data:string.txt`
say Compiler version #mcfunc_compiler_version

define standard_sword iron_sword`test_data:sword.nbt`

scoreboard players set @s points 0 
scoreboard objectives add points dummy "Total Points"

$var = 50
$var += 10
$var -= 5

define somevalue 3333
define somevalue2 #somevalue
say #somevalue2

$var = #somevalue
#$error = string
$var = null

points$mitchfizz05 = 69

if $tier < data$pending_tier say Pending tier!

give @s #standard_sword 1

summon minecraft:villager ~ ~ ~ `test_data:villager.nbt`

execute as @a run tellraw @s `test_data:hello.json`

if not bb$tier 2.. (
	$aaaa = 1
	say test
	
	execute if score tier data matches 4.. run (
		say nested if
	)
)
