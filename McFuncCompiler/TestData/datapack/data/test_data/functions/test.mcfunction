# Test
setblock ~ ~ ~ air

say `test_data:string.txt`

define test Hello.

define compiled_header File {file}

say Hello world
say Compiler version #mcfunc_compiler_version
say #test

define standard_sword iron_sword`test_data:sword.nbt`

scoreboard players set @s points 0 
scoreboard objectives add points dummy "Total Points"

give mitchfizz05 iron_sword`test_data:sword.nbt` 1

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

give @s #standard_sword 1

summon minecraft:villager ~ ~ ~ `test_data:villager.nbt`

execute as @a run tellraw @s `test_data:hello.json`

execute if score tier data matches 4.. run (
	$aaaa = 1
	say test
	
	execute if score tier data matches 4.. run (
		say nested if
	)
)
