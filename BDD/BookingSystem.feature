Feature: Admin login

w Scenario: Admin logs in successfully
        Given an admin user exists
        And the credentials are correct
        When the admin attempts to log in
        Then the login is successful
        
        
Feature: Insert property listing

w Scenario: Admin successfully adds a property
        Given an admin is logged in
        And the property data is valid
        When the admin inserts a new property
        Then the property is saved
        And the property is visible and available for booking.
