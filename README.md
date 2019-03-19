# pantrytracker
This repository is the greater PantryTracker food storage management system.


The intent of this network of applications is:

1) For end users to track what is in their pantry
2) Receive notifications when running low on commonly-consume products
3) Provide recipe ideas and low-cost grocery runs by using what is in their pantry
4) Easily add to their pantry by wiring up to their online grocery accounts
        * Potentials for wiring up:
            a) Amazon Prime Pantry
            b) Walmart Grocery Pickup & Order History
            c) Kroger Click-List
5) Easily deduct from their pantry by simply selecting which recipes they plan
6) Create shopping lists based on suggested or user-supplied items.
       * Additionally, this could integrate with other 3rd party list providers
            
            
            
Written as a micro-service architecture, this will consist of these basic components:

1) Centralized log-in (SSO) backed by Firebase account management & Cosmos DB for user profiles
2) Recipe API for management those desiring to upload and share recipes
3) Pantry API for management of inventory levels and associated notifications
4) Notification API for configuring notification types, devices, and mediums
5) Product API as a primary data source 
       * Kicking the can on this one, as there are many different sources of data for this:
              a) integrate with 3rd party grocery API
              b) generate own from user shopping lists, etc...
              c) generate own from recipes supplied by users
