GRANT ALL PRIVILEGES ON *.* TO 'gzz'@'%' WITH GRANT OPTION;  #赋予用户权限

ALTER USER 'gzz'@'%' IDENTIFIED BY 'password' PASSWORD EXPIRE NEVER; #修改加密规则 

ALTER USER 'gzz'@'%' IDENTIFIED WITH mysql_native_password BY 'gzz123'; #更新用户的密码 
