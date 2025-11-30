cat "$PSScriptRoot/chats.sql" | docker exec -i chat-chats-db psql -U admin -d chats 
cat "$PSScriptRoot/users.sql" | docker exec -i chat-chats-db psql -U admin -d chats 
cat "$PSScriptRoot/chat_users.sql" | docker exec -i chat-chats-db psql -U admin -d chats 
cat "$PSScriptRoot/chat_user_privileges.sql" | docker exec -i chat-chats-db psql -U admin -d chats 