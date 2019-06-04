# Test
setblock ~ ~ ~ air

define test Hello.

say Hello world
say Compiler version #mcfunc_compiler_version
say #test

define standard_sword iron_sword`test_data:sword.json`

scoreboard players set @s points 0 
scoreboard objectives add points dummy "Total Points"

$var = 50
$var += 10
$var -= 5

define somevalue 3333

$var = #somevalue
#$error = string
$var = null

points$mitchfizz05 = 69

give @s #standard_sword 1

execute as @a run tellraw @s `test_data:hello`



execute if score tier data matches 4.. run (
	$aaaa = 1
	say test
)
