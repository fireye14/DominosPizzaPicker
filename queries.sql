select * from PizzaPicker..PizzaView where IsRandomlygenerated = 1 and eaten = 1 and (topping1 = 'Black Olives' or topping2 = 'Black Olives' or topping3 = 'Black Olives')

select * from toppings
select * from Sauces


-- count of how many times each sauce appears on a pizza; this ist the same for all sauces
select s.Name, count(s.Name) from PizzaView v
inner join sauces s on v.Sauce = s.Name
where v.IsRandomlyGenerated = 1 
group by s.Name



-- count of how many pizzas have been eaten for each topping out of how many total pizzas contain each topping
select n.Topping, n.total as NumPizzasEaten, t.total as OfTotal from (
	select Name as Topping, count(*) as total
	from Toppings t
	inner join PizzaView p on t.Name = p.Topping1 or t.Name = p.Topping2 or t.Name = p.Topping3
	where p.Eaten = 1 and p.IsRandomlyGenerated = 1
	group by t.Name
) n
inner join (
	select Name as Topping, count(*) as total
	from Toppings t
	inner join PizzaView p on t.Name = p.Topping1 or t.Name = p.Topping2 or t.Name = p.Topping3
	where p.IsRandomlyGenerated = 1
	group by t.Name
) t on n.Topping = t.Topping


-- TODO: do the same as above, but with sauces
