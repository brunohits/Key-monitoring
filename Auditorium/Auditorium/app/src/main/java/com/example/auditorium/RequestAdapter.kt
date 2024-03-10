package com.example.auditorium

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView

class RequestAdapter(var requests: List<Request>,var context: Context ): RecyclerView.Adapter<RequestAdapter.MyViewHolder>() {
    class MyViewHolder(view: View): RecyclerView.ViewHolder(view){
        val title: TextView = view.findViewById(R.id.number_of_key)
        val status: TextView = view.findViewById(R.id.status)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): MyViewHolder {
       val view = LayoutInflater.from(parent.context).inflate(R.layout.request_in_list,parent,false)
        return MyViewHolder(view)
    }

    override fun getItemCount(): Int {
        return  requests.count()
    }

    override fun onBindViewHolder(holder: MyViewHolder, position: Int) {
       holder.title.text = requests[position].title
        holder.status.text = requests[position].status
    }
}