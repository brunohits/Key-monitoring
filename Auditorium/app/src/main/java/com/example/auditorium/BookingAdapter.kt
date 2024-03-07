package com.example.auditorium

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView

class BookingAdapter(var bookings : List<Booking>, var context: Context ): RecyclerView.Adapter<BookingAdapter.MyViewHolder>() {
    class MyViewHolder(view: View): RecyclerView.ViewHolder(view){
        val title: TextView = view.findViewById(R.id.booking_time)
        val status: TextView = view.findViewById(R.id.booking_status)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): MyViewHolder {
       val view = LayoutInflater.from(parent.context).inflate(R.layout.booking_item,parent,false)
        return MyViewHolder(view)
    }

    override fun getItemCount(): Int {
        return  bookings.count()
    }

    override fun onBindViewHolder(holder: MyViewHolder, position: Int) {
       holder.title.text = bookings[position].title
        holder.status.text = bookings[position].status
    }
}