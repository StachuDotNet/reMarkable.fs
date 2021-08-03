# this assumes you've already built, say, in an IDE

# delete remote sandbox
ssh root@10.11.99.1 rm -rf /home/root/reMarkable.fs.Demo

# copy
scp -r reMarkable.fs.Demo/bin/Debug/net5.0/ root@10.11.99.1:/home/root/reMarkable.fs.Demo

# ssh
ssh root@10.11.99.1
