package com.example.auditorium

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Button
import android.widget.Toast
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView

class BookingActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.booking)

        val bookingList:RecyclerView = findViewById(R.id.booking_list)
        val book = arrayListOf<Booking>()

                    
        book.add(Booking(1,"8:45-10:20","свободна"))
        book.add(Booking(2,"10:35-12:10:20","занята"))
        book.add(Booking(3,"12:25-14:00","свободна"))
        book.add(Booking(4,"14:45-16:20","свободна"))
        book.add(Booking(5,"16:35-18:10","занята"))
        book.add(Booking(6,"18:25-20:00","свободна"))

        bookingList.layoutManager = LinearLayoutManager(this)
        bookingList.adapter = BookingAdapter(book,this)


    }
}