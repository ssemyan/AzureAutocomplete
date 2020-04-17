set nocount on
go

drop table StreetAddresses
go

create table StreetAddresses
(
	Address varchar(255) not null primary key
)
go

insert into StreetAddresses (address) select '123 MAIN ST'
insert into StreetAddresses (address) select '456 UNION ST'
insert into StreetAddresses (address) select '145 SOUTH ST'
insert into StreetAddresses (address) select '543 NORTH ST'
insert into StreetAddresses (address) select '345 WEST ST'
